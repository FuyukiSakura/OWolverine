using Microsoft.Azure.Documents.Client;
using System;
using System.Threading.Tasks;
using OWolverine.Models.Ogame;
using Microsoft.Azure.Documents;
using System.Net;
using System.Linq;

namespace OWolverine.Services.Cosmos
{
    public class StarMapBLL
    {
        private const string DatabaseName = "StarMap";
        private const string CollectionName = "Servers";
        private static DocumentClient _client { get; set; } = new DocumentClient(new Uri(Environment.GetEnvironmentVariable("ENDPOINT_URI")), Environment.GetEnvironmentVariable("PRIMARY_KEY"));
        
        /// <summary>
        /// Create new server
        /// </summary>
        /// <param name="universe"></param>
        /// <returns></returns>
        public static async Task CreateServerDocumentIfNotExists(Universe universe)
        {
            try
            {
                await _client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseName, CollectionName, universe.Id));
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.NotFound)
                {
                    await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName), universe);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Get the full server list
        /// </summary>
        /// <returns></returns>
        public static Universe[] GetServerList()
        {
            return _client.CreateDocumentQuery<Universe>(
                UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName),
                "SELECT c.Name, c.ServerId, c.LastUpdate FROM c").ToArray();
        }
    }
}
