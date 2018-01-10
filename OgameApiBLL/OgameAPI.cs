using OgameApiBLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;

namespace OgameApiBLL
{
    public class OgameAPI
    {
        private const string playerAPI = "players.xml";
        private const string universeAPI = "universe.xml";
        private const string universes = "universes.xml";
        private const string serverDataAPI = "serverData.xml";
        private const string playerDataApi = "playerData.xml";

        /// <summary>
        /// Find player with given name
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
                /* Alliance = new Alliance
                {
                    Id = playerElement.Attribute("alliance").Value
                }, */
                Server = server
            };
        }

        public static void FillPlayerData(Player player)
        {
            var httpClient = new HttpClient();
            var result = httpClient.GetAsync(String.Format("https://{0}/api/{1}?id={2}",player.Server, playerDataApi,player.Id)).Result;
            var stream = result.Content.ReadAsStreamAsync().Result;

            var itemXml = XElement.Load(stream);
            //Load military info
            player.Data.Ships = (int)itemXml
                .Element("positions")
                .Elements("position")
                .FirstOrDefault(p => p.Attribute("type").Value == "3")
                .Attribute("ships");

            //Load planets
            List<Planet> planets = itemXml.Element("planets").Elements("planet").Select(planet => new Planet()
            {
                Id = planet.Attribute("id").Value,
                Name = planet.Attribute("name").Value,
                Coords = planet.Attribute("coords").Value
            }).ToList();
            player.Data.Planets = planets;
        }
    }
}
