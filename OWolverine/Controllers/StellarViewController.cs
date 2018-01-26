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
                await StarMapBLL.CreateServerDocumentIfNotExistsAsync(server);
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

            var universe = await StarMapBLL.GetServer(id);
            if (universe == null) return NotFound(); //Server not found

            //Load Alliance
            universe.Alliances = allianceList.Alliances;
            universe.AllianceLastUpdate = allianceList.LastUpdate;

            //Load Players
            universe.Players = playerList.Players;
            universe.PlayersLastUpdate = playerList.LastUpdate;
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
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Generate Score History
        /// </summary>
        /// <param name="type"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="ApiTime"></param>
        /// <returns></returns>
        private ScoreHistory GenerateScoreHistory (ScoreType type, int oldValue, int newValue, DateTime ApiTime)
        {
            return new ScoreHistory
            {
                Type = type.ToString(),
                OldValue = oldValue,
                NewValue = newValue,
                UpdatedAt = ApiTime
            };
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
