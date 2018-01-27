using Microsoft.Azure.Documents.Client;
using System;
using System.Threading.Tasks;
using CSharpUtilities;
using OWolverine.Models.Ogame;
using Microsoft.Azure.Documents;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using OWolverine.Models.StarMapViewModels;

namespace OWolverine.Services.Cosmos
{
    public class StarMapBLL
    {
        private const string DatabaseName = "StarMap";
        private const string CollectionName = "Servers";

        private const string ServerPrefix = "TW.Server.";
        private const string ScoreBoardPrefix = "TW.Score.";

        public static string GetServerId(int id) => ServerPrefix + id;
        public static string GetScoreBoardId(ScoreCategory category, int id) => 
            $"{ScoreBoardPrefix}{category.ToString()}.{id}";

        private static DocumentClient _client { get; set; } = new DocumentClient(new Uri(Environment.GetEnvironmentVariable("ENDPOINT_URI")), Environment.GetEnvironmentVariable("PRIMARY_KEY"));

        /// <summary>
        /// Create new server
        /// </summary>
        /// <param name="universe"></param>
        /// <returns></returns>
        public static async Task CreateUniverseIfNotExistsAsync(Universe universe)
        {
            await CreateServerDocumentIfNotExistsAsync(universe.Id, universe);
        }

        // ##### Servers
        /// <summary>
        /// Get the server data
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<Universe> GetServer(int id)
        {
            try
            {
                return await _client.ReadDocumentAsync<Universe>(UriFactory.CreateDocumentUri(DatabaseName, CollectionName, GetServerId(id)));
            }catch(DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Update the server list
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static async Task UpdateServerAsync(Universe item)
        {
            await _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseName, CollectionName, item.Id), item);
        }

        /// <summary>
        /// Get the full server list
        /// </summary>
        /// <returns></returns>
        public static Universe[] GetServerList()
        {
            return _client.CreateDocumentQuery<Universe>(
                UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName),
                "SELECT c.ServerId, c.Name, c.Speed, c.FleetSpeed, c.Statistic, " +
                "c.ActivePlayerCount, c.PlayerCount, c.MoonCount, c.PlanetCount, " +
                "c.PlayersLastUpdate, c.AllianceLastUpdate, c.PlanetsLastUpdate, " +
                $"c.LastUpdate FROM c WHERE SUBSTRING(c.id, 0, {ServerPrefix.Length}) = '{ServerPrefix}'").ToArray();
        }

        // ##### Planets
        /// <summary>
        /// Search planet by coordinate given range
        /// </summary>
        /// <param name="coords"></param>
        /// <param name="serverId"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static List<Planet> SearchPlanetsByCoordinate(Coordinate coords, int serverId, int range)
        {
            var queryString = $"SELECT VALUE planet FROM c JOIN p IN c.Players JOIN planet IN p.Planets JOIN planet.Coords crd WHERE c.id = '{GetServerId(serverId)}' ";
            if (coords.Galaxy != 0)
            {
                queryString += $"AND crd.Galaxy = {coords.Galaxy} ";
            }
            if (coords.System != 0)
            {
                var minSys = coords.System - range;
                var maxSys = coords.System + range;
                queryString += $"AND (crd.System BETWEEN {minSys} AND {maxSys}) ";
            }
            if (coords.Location != 0)
            {
                queryString += $"AND crd.Location = {coords.Location}";
            }
            return _client.CreateDocumentQuery<Planet>(
                UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName),
                queryString).ToList();
        }

        // ##### Players
        /// <summary>
        /// Search player by given name and server
        /// If serverId not given (or -1), search all servers
        /// </summary>
        /// <param name="name"></param>
        /// <param name="serverId"></param>
        /// <param name="caseSensitive"></param>
        /// <param name="strict"></param>
        /// <returns></returns>
        public static List<Player> SearchPlayerByName(string name, int serverId = -1, string status = "", bool caseSensitive = false, bool strict = false)
        {
            var query = _client.CreateDocumentQuery<Universe>(
                UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName))
                .Where(u => serverId == -1 ? u.Id.Contains(ServerPrefix) : u.Id == GetServerId(serverId))
                .SelectMany(u => u.Players);

            if (strict)
            {
                query = query.Where(p => p.Name == name);
            }
            else if (caseSensitive)
            {
                query = query.Where(p => p.Name.Contains(name));
            }
            else
            {
                query = query.Where(p => p.Name.ToLower().Contains(name.ToLower()));
            }
            
            foreach(var c in status)
            {
                //Mark all status
                query = query.Where(p => p.Status.Contains(c));
            }

            var players = query.ToList();
            var scoreDocumnet = GetScoreByIds(serverId, players.Select(p => p.Id).ToArray());
            foreach (var player in players)
            {
                //Populate score info
                var score = scoreDocumnet.FirstOrDefault(s => s.Id == player.Id);
                if (score != null)
                {
                    score.UpdateHistory = score.UpdateHistory
                        .GroupBy(h => h.Type)
                        .Select(g => g.OrderByDescending(h => h.UpdatedAt).First()).ToList();
                    player.Score = score;
                }
            }
            return players;
        }

        // ##### Scores
        /// <summary>
        /// Create new score document for player
        /// </summary>
        /// <param name="scoreBoard"></param>
        /// <returns></returns>
        public async static Task CreateScoreBoardIfNotAExistsAsync(ScoreBoard scoreBoard)
        {
            await CreateServerDocumentIfNotExistsAsync(scoreBoard.Id, scoreBoard);
        }

        /// <summary>
        /// Update the score
        /// </summary>
        /// <param name="scoreBoard"></param>
        /// <returns></returns>
        public async static Task UpdateScoreBoardAsync(ScoreBoard scoreBoard)
        {
            await _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseName, CollectionName, scoreBoard.Id), scoreBoard);
        }

        /// <summary>
        /// Get score board
        /// If not exists create new
        /// </summary>
        /// <param name="id"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public async static Task<ScoreBoard> GetScoreBoardAsync(int id, ScoreCategory category)
        {
            try
            {
                return await _client.ReadDocumentAsync<ScoreBoard>(UriFactory.CreateDocumentUri(DatabaseName, CollectionName, GetScoreBoardId(category, id)));
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    var scoreBoard = new ScoreBoard()
                    {
                        ServerId = id,
                        Category = category,
                        Scores = new List<Score>()
                    };
                    await CreateScoreBoardIfNotAExistsAsync(scoreBoard);
                    return scoreBoard;
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Get score by player Ids and server id
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="playerIds"></param>
        /// <returns></returns>
        public static Score[] GetScoreByIds(int serverId, int[] playerIds)
        {
            return _client.CreateDocumentQuery<ScoreBoard>(
                UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName))
                .Where(sb => sb.Id == GetScoreBoardId(ScoreCategory.Player, serverId))
                .SelectMany(sb => sb.Scores)
                .Where(s => playerIds.Contains(s.Id)).ToArray();
        }

        /* ---------- Private Helper function ---------- */
        /// <summary>
        /// Create document in cosmos DB
        /// </summary>
        /// <param name="id"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        private static async Task CreateServerDocumentIfNotExistsAsync(string id, object document)
        {
            try
            {
                await _client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseName, CollectionName, id));
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName), document);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
