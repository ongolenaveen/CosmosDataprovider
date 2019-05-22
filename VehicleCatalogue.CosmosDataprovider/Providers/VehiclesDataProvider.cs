using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using VehicleCatalogue.CosmosDataprovider.Entities;
using VehicleCatalogue.CosmosDataprovider.Interfaces;
using VehicleCatalogue.Domain.Interfaces;
using VehicleCatalogue.Domain.Models;

namespace VehicleCatalogue.CosmosDataprovider.Providers
{
    public class VehiclesDataProvider: IVehiclesDataProvider
    {
        private readonly ICosmosDbConnector _cosmosDbConnector;
        private readonly IMapper _autoMapper;
        public VehiclesDataProvider(IMapper autoMapper, ICosmosDbConnector cosmosDbConnector)
        {
            _autoMapper = autoMapper;
            _cosmosDbConnector = cosmosDbConnector;
        }

        public async Task<IEnumerable<VehicleDomainModel>> Search(VehicleSearchDomainModel searchCriteria)
        {
            var tenant = "Belgium-Belgium-BelgiumClient1";
            var entities = await _cosmosDbConnector.Search<VehicleEntity>(tenant, searchCriteria);
            var vehicleDomainModel = _autoMapper.Map<IEnumerable<VehicleDomainModel>>(entities);
            return vehicleDomainModel;
        }

        public async Task<VehicleDomainModel> Create(VehicleDomainModel vehicle)
        {
            var entity = _autoMapper.Map<VehicleEntity>(vehicle);
            entity.Tenant = "Belgium-Belgium-BelgiumClient1";
            var response = await _cosmosDbConnector.Upsert<VehicleEntity>(entity);
            var vehicleDomainModel = _autoMapper.Map<VehicleDomainModel>(response);
            return vehicleDomainModel;
        }
    }
}
