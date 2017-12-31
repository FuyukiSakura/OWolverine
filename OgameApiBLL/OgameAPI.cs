using OgameApiBLL.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;

namespace OgameApiBLL
{
    public class OgameAPI
    {
        private const string playerAPI = "players.xml";
        private const string universeAPI = "universe.xml";
        private const string playerDataApi = "playerData.xml";

        /// <summary>
        /// Find player XML item with given name
        /// </summary>
        /// <param name="server"></param>
        /// <param name="playerName"></param>
        /// <returns></returns>
        public static Player FindPlayer(string server, string playerName)
        {
            var httpClient = new HttpClient();
            var result = httpClient.GetAsync("https://" + server + "/api/" + playerAPI).Result;
            var stream = result.Content.ReadAsStreamAsync().Result;

            var itemXml = XElement.Load(stream);
            var playerElement = itemXml.Elements("player").SingleOrDefault(x => String.Equals(x.Attribute("name").Value, playerName, StringComparison.OrdinalIgnoreCase));
            if (playerElement == null) return null; //Plyaer not found
            return new Player
            {
                Id = playerElement.Attribute("id").Value,
                Name = playerElement.Attribute("name").Value,
                Alliance = playerElement.Attribute("alliance").Value
            };
        }
    }
}
