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
using OWolverine.Models.StellarView;
using OgameApiBLL;

namespace OWolverine.Controllers
{
    public class StellarViewController : Controller
    {
        private const string playerAPI = "players.xml";
        private const string universeAPI = "universe.xml";
        private const string playerDataApi = "playerData.xml";

        public IActionResult Index()
        {
            return View();
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
                data = player.Data.Planets
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

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
