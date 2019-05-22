using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VehicleCatalogue.Domain.Models;

namespace VehicleCatalogue.Domain.Interfaces
{
    public interface IVehiclesDataProvider
    {
        Task<IEnumerable<VehicleDomainModel>> Search(VehicleSearchDomainModel searchCriteria);

        Task<VehicleDomainModel> Create(VehicleDomainModel vehicle);
    }
}
