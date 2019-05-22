using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VehicleCatalogue.CosmosDataprovider.Entities;
using VehicleCatalogue.CosmosDataprovider.Interfaces;
using VehicleCatalogue.Domain.Models;

namespace VehicleCatalogue.CosmosDataprovider.Providers
{
    public class CosmosDbConnector: ICosmosDbConnector
    {
        private readonly IConfiguration _configuration;
        private readonly DocumentClient _documentClient;
        private string _databaseName;
        private string _collectionName;
        private readonly ILogger<CosmosDbConnector> _logger;

        public CosmosDbConnector(IConfiguration configuration, ILogger<CosmosDbConnector> logger)
        {
            _configuration = configuration;
            _logger = logger;
            var endpointUri = _configuration["Data:endpoint"];
            var primaryKey = _configuration["Data:authKey"];
            _databaseName = _configuration["Data:database"];
            _collectionName = _configuration["Data:collection"];
            _documentClient = new DocumentClient(new Uri(endpointUri), primaryKey);
            Task.Run(async () => await CreateDatabaseAndCollectionIfNotExistsAsync()).Wait();
        }

        /// <summary>
        /// Get Entity from the given table name for a given partition and row key
        /// </summary>
        /// <typeparam name="TResponse">Response Type</typeparam>
        /// <param name="tableName">Azure Cosmos Table Name</param>
        /// <param name="partitionId">Partition Id in the Table</param>
        /// <param name="rowId">Row Key in the Partition</param>
        /// <returns>Response Received From Azure</returns>
        public async Task<TResponse> Get<TResponse>(string partionId, string rowId) where TResponse : class,ICosmosEntity
        {
            _logger.LogInformation($"Getting Vehicle for {partionId} and rowid {rowId}");

            // Build a query to retrieve a Competition with a specific Row
            var collectionUri = UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName);
            var documentQuery = _documentClient.CreateDocumentQuery<TResponse>(collectionUri,
            new FeedOptions()
            {
                EnableCrossPartitionQuery = false,
                MaxItemCount = 1,
                PartitionKey = new PartitionKey(partionId)
            })
            .Where(c => c.RowId == rowId && c.Tenant == partionId)
            .Select(c => c)
            .AsDocumentQuery();

            var query = documentQuery.ToString();
            _logger.LogInformation($"Document Query: {query}");

            while (documentQuery.HasMoreResults)
            {
                foreach (var vehicle in await documentQuery.ExecuteNextAsync<TResponse>())
                {
                    return vehicle;
                }
            }

            // No matching document found
            return default(TResponse);
        }

        /// <summary>
        /// Search for a Vehicle
        /// </summary>
        /// <typeparam name="TResponse">Type of Response</typeparam>
        /// <param name="partionId">Partition Id</param>
        /// <param name="searchCriteria">Search Criteria</param>
        /// <returns>Vehicle Records Matching Search Criteria</returns>
        public async Task<IEnumerable<TResponse>> Search<TResponse>(string partitionId, VehicleSearchDomainModel searchCriteria) where TResponse : class, ICosmosEntity
        {
            _logger.LogInformation($"Searching For Vehicles in Partion {partitionId}");

            var response = new List<TResponse>();
            var collectionUri = UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName);
            var whereClause = PrepareWhereClause(partitionId, searchCriteria);
            var documentQuery = _documentClient.CreateDocumentQuery<TResponse>(collectionUri,
                new FeedOptions()
                {
                    EnableCrossPartitionQuery = false,
                    MaxItemCount = 100,
                })
                //.Where("Tenant == @0 and (Make.ToLower() == @1 or Make.ToLower() == @2) ", partionId, "nissan", "peugeot")
                .Where(whereClause)
                .Select(c => c)
                .AsDocumentQuery();

            var query = documentQuery.ToString();

            _logger.LogInformation($"Document Query: {query}");

            while (documentQuery.HasMoreResults)
            {
                var feedResponse = await documentQuery.ExecuteNextAsync<TResponse>();
                _logger.LogInformation($"Request Charge :{feedResponse.RequestCharge}");
                foreach (TResponse vehicle in feedResponse)
                {
                    response.Add(vehicle);
                }
            }
            return response;
        }

        /// <summary>
        /// Insert/Update Entity into the Table
        /// </summary>
        /// <typeparam name="TEntity">Entity To be Inserted</typeparam>
        /// <param name="tableName">Azure Cosmos Table Name</param>
        /// <param name="request">Request</param>
        /// <returns>Response Received From Azure</returns>
        public async Task<TEntity> Upsert<TEntity>(TEntity request) where TEntity : class,ICosmosEntity
        {
            var collectionUri = UriFactory.CreateDocumentCollectionUri(_databaseName, _collectionName);
            var documentResponse = await _documentClient.UpsertDocumentAsync(collectionUri, request);
            TEntity upsertedEntity = (dynamic)documentResponse.Resource;
            return upsertedEntity;
        }

        /// <summary>
        /// Create Database and Collection If Not Exists
        /// </summary>
        /// <returns>Task</returns>
        private async Task CreateDatabaseAndCollectionIfNotExistsAsync()
        {
            await _documentClient.CreateDatabaseIfNotExistsAsync(new Database { Id = _databaseName });
            var partitionKeyDefinition = new PartitionKeyDefinition() { Paths = new Collection<string>() { "/Tenant" } };
            var uniqueKey = new UniqueKey();
            uniqueKey.Paths.Add("/RowId");
            var uniqueKeyPolicy = new UniqueKeyPolicy { UniqueKeys = { uniqueKey } };
            await _documentClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(_databaseName), new DocumentCollection { Id = _collectionName , PartitionKey = partitionKeyDefinition , UniqueKeyPolicy = uniqueKeyPolicy }, new RequestOptions {});
        }

        /// <summary>
        /// Prepare Where Clause based on Search Criteria
        /// </summary>
        /// <param name="partitionId">Partion Id</param>
        /// <param name="searchCriteria">Search Criteria</param>
        /// <returns>Where Clause based on Search Criteria</returns>
        private string PrepareWhereClause(string partitionId, VehicleSearchDomainModel searchCriteria)
        {
            var and = "and" ;
            var or = "or ";
            var whereClause = $"Tenant == \"{partitionId}\"";
            var tempQuery = string.Empty;
            
            var makeQuery = AppendPredicate(searchCriteria.Make, "Make");
            makeQuery = ReplaceLastOccurrence(makeQuery, or, string.Empty);
            whereClause = (!string.IsNullOrWhiteSpace(makeQuery)) ? $"{whereClause} {and} {makeQuery}" : whereClause;

            var modelQuery = AppendPredicate(searchCriteria.Model, "Model");
            modelQuery = ReplaceLastOccurrence(modelQuery, or, string.Empty);
            whereClause = (!string.IsNullOrWhiteSpace(modelQuery)) ? $"{whereClause} {and} {modelQuery}" : whereClause;

            var bodyTypesQuery = AppendPredicate(searchCriteria.BodyTypes, "BodyType");
            bodyTypesQuery = ReplaceLastOccurrence(bodyTypesQuery, or, string.Empty);
            whereClause = (!string.IsNullOrWhiteSpace(bodyTypesQuery)) ? $"{whereClause} {and} {bodyTypesQuery}" : whereClause;

            var fuelTypesQuery = AppendPredicate(searchCriteria.FuelTypes, "FuelType");
            fuelTypesQuery = ReplaceLastOccurrence(fuelTypesQuery, or, string.Empty);
            whereClause = (!string.IsNullOrWhiteSpace(fuelTypesQuery)) ? $"{whereClause} {and} {fuelTypesQuery}" : whereClause;

            var gearBoxTypesQuery = AppendPredicate(searchCriteria.GearboxTypes, "GearBoxType");
            gearBoxTypesQuery = ReplaceLastOccurrence(gearBoxTypesQuery, or, string.Empty);
            whereClause = (!string.IsNullOrWhiteSpace(gearBoxTypesQuery)) ? $"{whereClause} {and} {gearBoxTypesQuery}" : whereClause;

            return whereClause;
        }

        /// <summary>
        /// Append Predicate
        /// </summary>
        Func<IEnumerable<string>,string, string> AppendPredicate = delegate (IEnumerable<string> elements, string columnName)
        {
            var tempQuery = string.Empty;
            var or = "or ";

            if (elements == null || !elements.Any())
                return string.Empty;

            elements.ToList().ForEach(x => { tempQuery = tempQuery + $"{columnName}.ToLower() == \"{ x.ToLower()}\" {or}"; });
            return $"({tempQuery})";
        };

        /// <summary>
        /// Replace Last Occurance Of a string with another string
        /// </summary>
        /// <param name="Source">Source String</param>
        /// <param name="Find">String to be Replaced</param>
        /// <param name="Replace">Replace With</param>
        /// <returns>Updated String</returns>
        private string ReplaceLastOccurrence(string Source, string Find, string Replace)
        {
            int place = Source.LastIndexOf(Find);

            if (place == -1)
                return Source;

            string result = Source.Remove(place, Find.Length).Insert(place, Replace);
            return result;
        }
    }
}
