using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OWolverine.Models;
using System.Net.Http;
using System.Xml.Linq;
using OWolverine.Models.Utility;
using OWolverine.Models.Ogame;
using OgameApiBLL;
using Microsoft.AspNetCore.Identity;
using OWolverine.Data;
using Microsoft.EntityFrameworkCore;
using OWolverine.Services.Ogame;
using CSharpUtilities;
using OWolverine.Models.StarMapViewModels;
using Microsoft.AspNetCore.Http;

namespace OWolverine.Controllers
{
    public class StellarViewController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        private const string playerAPI = "players.xml";
        private const string universeAPI = "universe.xml";
        private const string playerDataApi = "playerData.xml";

        //Sesssion
        private const string SessionServerSelection = "_ServerSelection";

        /// <summary>
        /// Dependency Injection
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        public StellarViewController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Get server list and random target on Index
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var vm = new StarIndexViewModel(await _context.Universes
                .Include(u => u.Players)
                .Include(u => u.Planets)
                    .ThenInclude(p => p.Moon)
                .ToArrayAsync());
            var lastSelection = HttpContext.Session.GetInt32(SessionServerSelection);
            if (lastSelection != null)
            {
                vm.SearchViewModel.ServerId = (int)lastSelection;
            }
            return View(vm);
        }

        /// <summary>
        /// Search player from database
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(StarSearchViewModel vm)
        {
            var universeList = _context.Universes
                .Include(u => u.Players)
                .AsNoTracking();
            var sivm = new StarIndexViewModel(await universeList.ToArrayAsync());
            //Save server selection
            HttpContext.Session.SetInt32(SessionServerSelection, vm.ServerId);

            //Start searching
            if (ModelState.IsValid)
            {
                //Clean up
                vm.PlayerName = vm.PlayerName ?? ""; //Prevent empty name

                //Return info from request
                sivm.SearchViewModel.PlayerName = vm.PlayerName;
                sivm.SearchViewModel.ServerId = vm.ServerId;
                sivm.SearchViewModel.Coords = vm.Coords;

                var universe = _context.Universes
                    .Include(u => u.Players)
                        .ThenInclude(p => p.Alliance)
                    .Include(u => u.Players)
                        .ThenInclude(player => player.Planets)
                        .ThenInclude(planet => planet.Moon)
                    .Include(u => u.Planets)
                    .AsNoTracking()
                    .First(u => u.Id == vm.ServerId);

                var players = new List<Player>();
                var planets = new List<Planet>();
                if (vm.Coords.IsAddress)
                {
                    //Targetted Search
                    var targetPlanet = universe.Planets.FirstOrDefault(e => e.Coords.IsEqual(vm.Coords));
                    if (targetPlanet != null)
                    {
                        players.Add(targetPlanet.Owner);
                        planets = new List<Planet>()
                        {
                            targetPlanet
                        };

                        //Search all planets owned by alliance member in range
                        planets.AddRange(universe.Planets
                            .Where(e => e.Owner.Alliance == targetPlanet.Owner.Alliance && 
                                e.Coords.Galaxy == targetPlanet.Coords.Galaxy && 
                                e.Coords.System >= (targetPlanet.Coords.System - vm.Range) &&
                                e.Coords.System <= (targetPlanet.Coords.System + vm.Range)
                            )
                        );
                        
                        foreach(var planet in planets)
                        {
                            //Add found players to list
                            if (!players.Contains(planet.Owner)) { 
                                players.Add(planet.Owner);
                            }
                        }
                    }
                }
                else
                {

                    //Normal Search
                    players = universe
                        .Players
                        .Where(e => e.Name.Contains(vm.PlayerName, StringComparison.OrdinalIgnoreCase)).ToList();

                    // ----- Status filter
                    if (vm.PlayerStatus.IsBanned)
                    {
                        players.RemoveAll(p => !p.IsBanned);
                    }
                    if (vm.PlayerStatus.IsFlee)
                    {
                        players.RemoveAll(p => !p.IsFlee);
                    }
                    if (vm.PlayerStatus.IsInactive)
                    {
                        players.RemoveAll(p => !p.IsInactive);
                    }
                    if (vm.PlayerStatus.IsLeft)
                    {
                        players.RemoveAll(p => !p.IsLeft);
                    }

                    // ----- Planet search
                    if (!vm.Coords.IsEmpty)
                    {
                        var requestedCoords = vm.Coords;
                        planets = universe.Planets.Where(e =>
                            (requestedCoords.Galaxy == 0 || e.Coords.Galaxy == requestedCoords.Galaxy) &&
                            (requestedCoords.System == 0 ||
                                e.Coords.System == requestedCoords.System ||
                                e.Coords.System >= (requestedCoords.System - vm.Range) &&
                                e.Coords.System <= (requestedCoords.System + vm.Range)
                                ) &&
                            (requestedCoords.Location == 0 || e.Coords.Location == requestedCoords.Location)
                        ).OrderBy(e => e.Coords.Galaxy)
                        .OrderBy(e => e.Coords.System)
                        .OrderBy(e => e.Coords.Location).ToList();

                        //Remove all planets that are not owned by a player
                        planets.RemoveAll(e => !players.Contains(e.Owner));

                        //Remove all players that does not own a planet in the range
                        players.RemoveAll(e => !e.Planets.Where(p => planets.Contains(p)).Any());
                    }
                }
                sivm.Planets = planets;
                sivm.Players = players;
                sivm.IsSearch = true;
            }
            return View("Index", sivm);
        }

        /// <summary>
        /// Refresh universe data
        /// </summary>
        /// <param name="id"></param>
        public async Task<IActionResult> RefreshUniverse(int id)
        {
            var universe = _context.Universes
                .Include(u => u.Players)
                .Include(u => u.Alliances)
                    .ThenInclude(a => a.Members)
                .Include(u => u.Planets)
                    .ThenInclude(p => p.Moon)
                .FirstOrDefault(u => u.Id == id);
            if (universe == null) return NotFound();

            //Load players into DB
            var playersData = OgameApi.GetAllPlayers(id);
            var playerList = playersData.Players;
            var playersLastUpdate = DateTimeHelper.UnixTimeStampToDateTime(playersData.LastUpdate);
            if (playersLastUpdate != universe.PlayersLastUpdate)
            {
                //Only update if the API Date is different
                var removeList = new List<Player>();
                foreach(var player in universe.Players)
                {
                    var pNew = playerList.FirstOrDefault(p => p.PlayerId == player.PlayerId);
                    if(pNew == null)
                    {
                        //Remove player if not found in API
                        removeList.Add(player);
                    }
                    else
                    {
                        player.Update(pNew);
                        playerList.Remove(pNew); //Remove from list after update
                    }
                }
                _context.RemoveRange(removeList);
                await _context.AddRangeAsync(playerList);
                universe.PlayersLastUpdate = playersLastUpdate; //Update API Date
                await _context.SaveChangesAsync();
            }

            //Load Alliance into DB
            var allianceData = OgameApi.GetAllAlliances(id);
            var allianceList = allianceData.Alliances;
            var alliancesLastUpdate = DateTimeHelper.UnixTimeStampToDateTime(allianceData.LastUpdate);
            if (alliancesLastUpdate != universe.AllianceLastUpdate)
            {
                //Only update if the API Date is different
                var removeList = new List<Alliance>();
                allianceList.ForEach(a =>
                {
                    var dbItem = universe.Alliances.FirstOrDefault(e => e.AllianceId == a.AllianceId);
                    a.Founder = universe.Players.FirstOrDefault(p => p.PlayerId == a.FounderId);
                    if (a.Founder == null)
                    {
                        //Unset founder ID if founder not found
                        a.FounderId = null;
                    }
                    else
                    {
                        //Assign ID of player in DB
                        a.FounderId = a.Founder.Id;
                    }

                    for (var i = 0; i < a.Members.Count; i++)
                    {
                        //Replace member placeholder with database object
                        a.Members[i] = universe.Players.FirstOrDefault(p => p.PlayerId == a.Members[i].PlayerId);
                    }
                    a.Members.RemoveAll(m => m == null);

                    if (dbItem != null)
                    {
                        a.Id = dbItem.Id;
                        //Update the context
                        dbItem.Update(a);
                        removeList.Add(a);
                    }
                });
                //Remove alliances no longer exists
                universe.Alliances.RemoveAll(e => !allianceList.Any(a => a.Id == e.Id));
                allianceList.RemoveAll(a => removeList.Contains(a));
                await _context.AddRangeAsync(allianceList); //Add new alliances
                universe.AllianceLastUpdate = alliancesLastUpdate; //Update API Date
                await _context.SaveChangesAsync();
            }

            //Load Planet into DB
            var planetData = OgameApi.GetAllPlanets(id);
            var planetList = planetData.Planets;
            var planetLastUpdate = DateTimeHelper.UnixTimeStampToDateTime(planetData.LastUpdate);
            if (planetLastUpdate != universe.PlanetsLastUpdate)
            {
                //Only update if the API Date is different
                //Update existing planets
                var removeList = new List<Planet>();
                planetList.ForEach(p =>
                {
                    //Assign real owner ID
                    var owner = universe.Players.FirstOrDefault(e => e.PlayerId == p.OwnerId);
                    if (owner == null) {
                        //Player removed planet no longer exists
                        removeList.Add(p);
                    }
                    else
                    {
                        p.OwnerId = owner.Id;
                    }
                });
                planetList.RemoveAll(p => removeList.Contains(p));

                removeList.Clear();
                foreach (var planet in universe.Planets)
                {
                    var pNew = planetList.FirstOrDefault(p => p.PlanetId == planet.PlanetId);
                    if (pNew == null)
                    {
                        //Remove if not exists in API
                        removeList.Add(planet);
                    }
                    else
                    {
                        planet.Update(pNew);
                        planetList.Remove(pNew); //Remove updated item
                    }
                }
                //Remove planets no longer exists
                _context.RemoveRange(removeList);
                await _context.AddRangeAsync(planetList);
                universe.PlanetsLastUpdate = planetLastUpdate;
            }
            universe.LastUpdate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetUser(string server,string playerName)
        {
            var player = OgameAPI.FindPlayer(server, playerName);
            if(player == null)
            {
                return new JsonResult(new Response() { status = APIStatus.Fail, message = "玩家不存在" });
            }

            OgameAPI.FillPlayerData(player);            
            return new JsonResult(new Response() {
                status = APIStatus.Success,
                message = "Returned with data",
                data = new[] { player.Data }
            });
        }

        [HttpGet]
        public JsonResult GetServers()
        {
            var httpClient = new HttpClient();
            var result = httpClient.GetAsync("https://s101-tw.ogame.gameforge.com/api/universes.xml").Result;
            var stream = result.Content.ReadAsStreamAsync().Result;
            var itemXml = XElement.Load(stream);
            var servers = itemXml.Elements("universe");
            return new JsonResult(false);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
