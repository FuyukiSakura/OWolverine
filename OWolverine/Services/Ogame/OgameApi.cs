using OWolverine.Models.Ogame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using CSharpUtilities;

namespace OWolverine.Services.Ogame
{
    public static class OgameApi
    {
        private const string playerAPI = "players.xml";
        private const string universeAPI = "universe.xml";
        private const string universes = "universes.xml";
        private const string serverDataAPI = "serverData.xml";
        private const string playerDataApi = "playerData.xml";

        private const string mainServer = "101";

        public static Universe[] GetAllUniverses()
        {
            var servers = XElement.Load(RequestAPI(mainServer, universes))
                .Elements("universe").Select(x => Convert.ToInt32(x.Attribute("id").Value)).ToArray();

            //Load server details
            var serializer = new XmlSerializer(typeof(UniverseXml));
            var universeList = new List<Universe>();
            foreach (var id in servers)
            {
                var universe = ObjectHelper.CloneAndUpcast<Universe>(
                   (UniverseXml)serializer.Deserialize(RequestAPI(id.ToString(), serverDataAPI))
                );
                universe.LastUpdate = DateTime.Now;
                universeList.Add(universe);
            }
            return universeList.ToArray();
        }

        /// <summary>
        /// Request ogame api
        /// </summary>
        /// <param name="server">Server to request from</param>
        /// <param name="api">The api to call</param>
        /// <param name="param">Extra param</param>
        /// <returns></returns>
        private static Stream RequestAPI(string serverId, string api, string param = "")
        {
            var httpClient = new HttpClient();
            var result = httpClient.GetAsync($"https://s{serverId}-tw.ogame.gameforge.com/api/{api}{param}").Result;
            return result.Content.ReadAsStreamAsync().Result;
        }
    }
}
