using System.Threading.Tasks;
using VehicleCatalogue.Domain.Models;
using System.Collections.Generic;

namespace VehicleCatalogue.Services
{
    public interface IVehicleService
    {
        /// <summary>
        /// Search For Vehicles
        /// </summary>
        /// <returns>Vehicles Matching Search Criteria</returns>
        Task<IEnumerable<VehicleDomainModel>> Search(VehicleSearchDomainModel searchCriteria);


        Task<VehicleDomainModel> Create(VehicleDomainModel vehicle);
    }
}
