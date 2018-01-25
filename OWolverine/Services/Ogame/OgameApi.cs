using CSharpUtilities;
using OWolverine.Models.Ogame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace OWolverine.Services.Ogame
{
    public static class OgameApi
    {
        private const string playerAPI = "players.xml";
        private const string allianceAPI = "alliances.xml";
        private const string universeAPI = "universe.xml";
        private const string universes = "universes.xml";
        private const string serverDataAPI = "serverData.xml";
        private const string playerDataApi = "playerData.xml";
        private const string highScoreApi = "highscore.xml";

        private const int mainServer = 101;

        /// <summary>
        /// Get all universe in the server of the origin
        /// </summary>
        /// <returns></returns>
        public static Universe[] GetAllUniverses()
        {
            var servers = XElement.Load(RequestAPI(mainServer, universes))
                .Elements("universe").Select(x => Convert.ToInt32(x.Attribute("id").Value)).ToArray();

            //Load server details
            var serializer = new XmlSerializer(typeof(Universe));
            var universeList = new List<Universe>();
            foreach (var id in servers)
            {
                var universe = (Universe)serializer.Deserialize(RequestAPI(id, serverDataAPI));
                universe.LastUpdate = DateTimeHelper.UnixTimeStampToDateTime(universe.Timestamp);
                universeList.Add(universe);
            }
            return universeList.ToArray();
        }

        /// <summary>
        /// Get all players in the given server
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public static PlayerList GetAllPlayers(int serverId)
        {
            var serializer = new XmlSerializer(typeof(PlayerList));
            var playerList = ((PlayerList)serializer.Deserialize(RequestAPI(serverId, playerAPI)));
            playerList.Players.ForEach(p => p.CreatedAt = playerList.LastUpdate);
            return playerList;
        }

        /// <summary>
        /// Get all alliances in server
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public static AllianceList GetAllAlliances(int serverId)
        {
            var serializer = new XmlSerializer(typeof(AllianceList));
            var allianceList = ((AllianceList)serializer.Deserialize(RequestAPI(serverId, allianceAPI)));
            allianceList.Alliances.ForEach(a =>
            {
                a.ServerId = serverId;
            }); //Assign server id
            return allianceList;
        }

        /// <summary>
        /// Get all planets in a server
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public static PlanetList GetAllPlanets(int serverId)
        {
            var serializer = new XmlSerializer(typeof(PlanetList));
            var planetList = ((PlanetList)serializer.Deserialize(RequestAPI(serverId, universeAPI)));
            planetList.Planets.ForEach(a =>
            {
                a.ServerId = serverId;
            }); //Assign server id
            return planetList;
        }

        /// <summary>
        /// Get high score in the given server
        /// </summary>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public static HighScore GetHighScore(int serverId, ScoreCategory category, ScoreType type)
        {
            var serializer = new XmlSerializer(typeof(HighScore));
            return ((HighScore)serializer.Deserialize(RequestAPI(serverId, highScoreApi, $"?category={(int)category}&type={(int)type}")));
        }

        /// <summary>
        /// Request ogame api
        /// </summary>
        /// <param name="server">Server to request from</param>
        /// <param name="api">The api to call</param>
        /// <param name="param">Extra param</param>
        /// <returns></returns>
        private static Stream RequestAPI(int serverId, string api, string param = "")
        {
            var httpClient = new HttpClient();
            var result = httpClient.GetAsync($"https://s{serverId.ToString()}-tw.ogame.gameforge.com/api/{api}{param}").Result;
            return result.Content.ReadAsStreamAsync().Result;
        }
    }
}
