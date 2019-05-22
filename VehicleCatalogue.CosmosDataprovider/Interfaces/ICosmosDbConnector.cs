using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleCatalogue.CosmosDataprovider.Entities;
using VehicleCatalogue.Domain.Models;

namespace VehicleCatalogue.CosmosDataprovider.Interfaces
{
    public interface ICosmosDbConnector
    {
        /// <summary>
        /// Get Entity from the given table name for a given partition and row key
        /// </summary>
        /// <typeparam name="TResponse">Response Type</typeparam>
        /// <param name="partitionId">Partition Id in the Table</param>
        /// <param name="rowId">Row Key in the Partition</param>
        /// <returns>Response Received From Azure</returns>
        Task<TResponse> Get<TResponse>(string partionId,string rowId) where TResponse : class,ICosmosEntity;

        /// <summary>
        /// Insert/Update Entity into the Table
        /// </summary>
        /// <typeparam name="TEntity">Entity To be Inserted</typeparam>
        /// <param name="request">Request</param>
        /// <returns>Response Received From Azure</returns>
        Task<TEntity> Upsert<TEntity>(TEntity request) where TEntity : class,ICosmosEntity;

        /// <summary>
        /// Search for a Vehicle
        /// </summary>
        /// <typeparam name="TResponse">Type of Response</typeparam>
        /// <param name="partionId">Partition Id</param>
        /// <param name="searchCriteria">Search Criteria</param>
        /// <returns>Vehicle Records Matching Search Criteria</returns>
        Task<IEnumerable<TResponse>> Search<TResponse>(string partionId, VehicleSearchDomainModel searchCriteria) where TResponse : class, ICosmosEntity;

    }
}
