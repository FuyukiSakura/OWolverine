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

            return new JsonResult(player);
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
