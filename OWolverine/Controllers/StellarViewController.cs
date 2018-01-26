using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OWolverine.Models;
using OWolverine.Models.Ogame;
using Microsoft.AspNetCore.Identity;
using OWolverine.Data;
using OWolverine.Services.Ogame;
using CSharpUtilities;
using OWolverine.Models.StarMapViewModels;
using Microsoft.AspNetCore.Http;
using OWolverine.Services.Cosmos;

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
        public IActionResult Index()
        {
            return View(new StarIndexViewModel(StarMapBLL.GetServerList()));
        }

        /// <summary>
        /// Search player from database
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(StarSearchViewModel vm)
        {
            return View("Index");
        }

        /// <summary>
        /// Update the server list
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> UpdateServerList()
        {
            var servers = OgameApi.GetAllUniverses();
            foreach (var server in servers)
            {
                await StarMapBLL.CreateUniverseIfNotExistsAsync(server);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Refresh universe data
        /// </summary>
        /// <param name="id"></param>
        public async Task<IActionResult> UpdateUniverse(int id)
        {
            var playerList = OgameApi.GetAllPlayers(id);
            var allianceList = OgameApi.GetAllAlliances(id);
            var planetList = OgameApi.GetAllPlanets(id);

            var universe = await StarMapBLL.GetServer(id);
            if (universe == null) return NotFound(); //Server not found

            //Load Alliance
            universe.Alliances = allianceList.Alliances;
            universe.AllianceLastUpdate = allianceList.LastUpdate;

            //Load Players
            universe.Players = playerList.Players;
            universe.PlayersLastUpdate = playerList.LastUpdate;

            //Load Planets
            foreach(var planet in planetList.Planets)
            {
                var owner = universe.Players.FirstOrDefault(p => p.Id == planet.OwnerId);
                if (owner == null) continue; //Ignore if owner not in player list

                var playerPlanet = owner.Planets.FirstOrDefault(pn => pn.Id == planet.Id);
                if(playerPlanet == null)
                {
                    //Add if not exist
                    owner.Planets.Add(planet);
                }
                else
                {
                    //Update if found
                    playerPlanet.Update(planet);
                }
            }
            universe.PlanetsLastUpdate = planetList.LastUpdate;
            universe.Statistic.MapUpdateDay = planetList.LastUpdate.ToString("ddd");
            await StarMapBLL.UpdateServerAsync(universe);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Update player scores
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> UpdateScoreBoard(int id)
        {
            var totalScoreData = OgameApi.GetHighScore(id, ScoreCategory.Player, ScoreType.Total);
            var econScoreData = OgameApi.GetHighScore(id, ScoreCategory.Player, ScoreType.Total);
            var researchScoreData = OgameApi.GetHighScore(id, ScoreCategory.Player, ScoreType.Research);
            var militaryScoreData = OgameApi.GetHighScore(id, ScoreCategory.Player, ScoreType.Research);
            var lastUpdate = DateTimeHelper.GetLatestDate(new List<DateTime>
            {
                totalScoreData.LastUpdate,
                econScoreData.LastUpdate,
                researchScoreData.LastUpdate,
                militaryScoreData.LastUpdate
            });

            var scoreBoard = await StarMapBLL.GetScoreBoardAsync(id, ScoreCategory.Player);
            if (scoreBoard.LastUpdate == lastUpdate) return RedirectToAction("Index"); //Abort if Api not updated
            // Update total
            foreach (var scoreData in totalScoreData.Scores)
            {
                SetScore(scoreBoard, scoreData.Id, scoreData.Value, ScoreType.Total.ToString(), totalScoreData.LastUpdate);
            }
            // Update Economy
            foreach (var scoreData in econScoreData.Scores)
            {
                SetScore(scoreBoard, scoreData.Id, scoreData.Value, ScoreType.Economy.ToString(), econScoreData.LastUpdate);
            }
            // Update Research
            foreach (var scoreData in researchScoreData.Scores)
            {
                SetScore(scoreBoard, scoreData.Id, scoreData.Value, ScoreType.Research.ToString(), researchScoreData.LastUpdate);
            }
            // Update Military
            foreach (var scoreData in militaryScoreData.Scores)
            {
                SetScore(scoreBoard, scoreData.Id, scoreData.Value, ScoreType.Military.ToString(), militaryScoreData.LastUpdate);
                SetScore(scoreBoard, scoreData.Id, scoreData.Ships, "ShipNumber", militaryScoreData.LastUpdate);
            }
            await StarMapBLL.UpdateScoreBoardAsync(scoreBoard);
            return RedirectToAction("Index");
        }

        private void SetScore(ScoreBoard scoreBoard, int playerId, int newValue, string type, DateTime lastUpdate)
        {
            var score = scoreBoard.Scores.FirstOrDefault(s => s.Id == playerId);
            if (score == null)
            {
                //Create new score item if not exists
                score = new Score()
                {
                    Id = playerId,
                    UpdateHistory = new List<ScoreHistory>()
                };
                scoreBoard.Scores.Add(score);
            }

            //Update score
            var typeInfo = score.GetType().GetProperty(type.ToString());
            var value = (int)typeInfo.GetValue(score);
            if (value != newValue)
            {
                //Add history
                var updateHistory = new ScoreHistory()
                {
                    Type = type,
                    OldValue = score.Total,
                    NewValue = newValue,
                    UpdateInterval = lastUpdate - score.LastUpdate,
                    UpdatedAt = lastUpdate
                };
                score.UpdateHistory.Add(updateHistory);

                //Update score
                typeInfo.SetValue(score, newValue);
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
