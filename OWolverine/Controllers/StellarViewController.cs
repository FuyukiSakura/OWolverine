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

namespace OWolverine.Controllers
{
    public class StellarViewController : Controller
    {
        private string playerAPI = "players.xml";
        private string universeAPI = "universe.xml";

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetUser(string server,string player)
        {
            var httpClient = new HttpClient();
            var result = httpClient.GetAsync("https://"+server+"/api/"+playerAPI).Result;
            var stream = result.Content.ReadAsStreamAsync().Result;

            var itemXml = XElement.Load(stream);
            var playerItem = itemXml.Elements("player").SingleOrDefault(x => (string)x.Attribute("name") == player);
            if(playerItem == null)
            {
                return new JsonResult(new Response() { status = APIStatus.Fail, message = "玩家不存在" });
            }

            var playerId = (int) playerItem.Attribute("id");
            result = httpClient.GetAsync("https://" + server + "/api/" + universeAPI).Result;
            stream = result.Content.ReadAsStreamAsync().Result;
            itemXml = XElement.Load(stream);
            List<Planet> planets = itemXml.Elements("planet").Where(x => (int)x.Attribute("player") == playerId).Select(planet => new Planet() {
                Name = (string)planet.Attribute("name"),
                Coords = (string)planet.Attribute("coords")
            }).ToList();
            
            return new JsonResult(new Response() {
                status = APIStatus.Success,
                message = "Returned with data",
                data = planets
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
