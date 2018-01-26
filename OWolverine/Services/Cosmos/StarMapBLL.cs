using Microsoft.Azure.Documents.Client;
using System;
using System.Threading.Tasks;
using OWolverine.Models.Ogame;
using Microsoft.Azure.Documents;
using System.Net;
using System.Linq;
using System.Collections.Generic;

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
                "SELECT c.ServerId, c.Name, " +
                "c.ActivePlayerCount, c.PlayerCount, c.MoonCount, c.PlanetCount, " +
                "c.PlayersLastUpdate, c.AllianceLastUpdate, c.PlanetsLastUpdate, " +
                $"c.LastUpdate FROM c WHERE SUBSTRING(c.id, 0, {ServerPrefix.Length}) = '{ServerPrefix}'").ToArray();
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
