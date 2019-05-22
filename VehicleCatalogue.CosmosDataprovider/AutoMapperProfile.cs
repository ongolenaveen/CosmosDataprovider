using AutoMapper;
using VehicleCatalogue.CosmosDataprovider.Entities;
using VehicleCatalogue.Domain.Models;

namespace VehicleCatalogue.CosmosDataprovider
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<VehicleDomainModel, VehicleEntity>()
                .ForMember(x => x.RowId, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.Tenant, opt => opt.Ignore());

            CreateMap<VehicleEntity, VehicleDomainModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.RowId))
;
        }
    }
}
