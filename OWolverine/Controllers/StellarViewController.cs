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

        public async Task<IActionResult> Index()
        {
            return View(await _context.Universes
                .Include(u => u.Players)
                .ToListAsync());
        }

        public async Task<IActionResult> RefreshUniverse(int id)
        {
            var universe = _context.Universes.FirstOrDefault(u => u.Id == id);
            if (universe == null)
            {
                return NotFound();
            }

            //Load players into DB
            var playersData = OgameApi.GetAllPlayers(id);
            var playerList = playersData.Players;
            var playersLastUpdate = DateTimeHelper.UnixTimeStampToDateTime(playersData.LastUpdate);
            if (playersLastUpdate != universe.PlayersLastUpdate)
            {
                //Only update if the API Date is different
                foreach (var player in playerList)
                {
                    var dbItem = _context.Players.FirstOrDefault(e => e.PlayerId == player.PlayerId && e.ServerId == player.ServerId);
                    if (dbItem == null)
                    {
                        //New object
                        player.Id = 0;
                    }
                    else
                    {
                        //Assign ID if exist in database
                        player.Id = dbItem.Id;
                        //Update the context
                        dbItem.Update(player);
                    }
                }
                //Remove players no longer exist
                _context.Players.RemoveRange(_context.Players.Where(
                    e => !playerList.Any(p => e.Id == p.Id)));
                _context.AddRange(playerList.Where(p => p.Id == 0)); //Add new players
                universe.PlayersLastUpdate = playersLastUpdate; //Update API Date
                await _context.SaveChangesAsync();
            }

            //Load Alliance into DB
            var allianceData = OgameApi.GetAllAlliance(id);
            var allianceList = allianceData.Alliances;
            var alliancesLastUpdate = DateTimeHelper.UnixTimeStampToDateTime(allianceData.LastUpdate);
            if (alliancesLastUpdate != universe.AllianceLastUpdate)
            {
                _context.Alliances.Include(e => e.Members); //Load member list
                allianceList.ForEach(a =>
                {
                    var dbItem = _context.Alliances.FirstOrDefault(e => e.AllianceId == a.AllianceId && e.ServerId == a.ServerId);
                    a.Founder = _context.Players.FirstOrDefault(p => p.PlayerId == a.FounderId && p.ServerId == a.ServerId);
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
                        a.Members[i] = _context.Players.FirstOrDefault(p => p.PlayerId == a.Members[i].PlayerId && p.ServerId == a.Members[i].ServerId);
                    }

                    if (dbItem == null)
                    {
                        //New object
                        a.Id = 0;
                    }
                    else
                    {
                        //Assign ID if exist in database
                        a.Id = dbItem.Id;
                        //Update the context
                        dbItem.Update(a);
                    }
                });
                //Remove alliances no longer exist
                _context.Alliances.RemoveRange(_context.Alliances.Where(
                    e => !allianceList.Any(a => a.Id == e.Id)));
                _context.AddRange(allianceList.Where(a => a.Id == 0)); //Add new alliances
                universe.AllianceLastUpdate = alliancesLastUpdate; //Update API Date
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
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
