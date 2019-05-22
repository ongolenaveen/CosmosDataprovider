using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleCatalogue.Domain.Interfaces;
using VehicleCatalogue.Domain.Models;

namespace VehicleCatalogue.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehiclesDataProvider _vehiclesDataProvider;

        public VehicleService(IVehiclesDataProvider vehiclesDataProvider)
        {
            _vehiclesDataProvider = vehiclesDataProvider;
        }

        /// <summary>
        /// Search For Vehicles
        /// </summary>
        /// <returns>Vehicle Details</returns>
        public async Task<IEnumerable<VehicleDomainModel>> Search(VehicleSearchDomainModel searchCriteria)
        {
            var response = await _vehiclesDataProvider.Search(searchCriteria);
            return response;
        }

        public async Task<VehicleDomainModel> Create(VehicleDomainModel vehicle)
        {
            var response = await _vehiclesDataProvider.Create(vehicle);
            return response;
        }
    }
}
