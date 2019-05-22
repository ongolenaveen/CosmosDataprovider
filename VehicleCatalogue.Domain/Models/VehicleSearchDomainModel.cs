using System.Collections.Generic;
using VehicleCatalogue.Domain.Enumerations;

namespace VehicleCatalogue.Domain.Models
{
    public class VehicleSearchDomainModel
    {
        public string Id { get; set; }

        public List<string> Make { get; set; }

        public List<string> Model { get; set; }

        public decimal? MinimumPrice { get; set; }

        public decimal? MaximumPrice { get; set; }

        public int? ModelYear { get; set; }

        public List<string> FuelTypes { get; set; }

        public List<string> GearboxTypes { get; set; }

        public int? MinimumMileage { get; set; }

        public int? MaximumMileage { get; set; }

        public List<string> Locations { get; set; }

        public List<string> BodyTypes { get; set; }

        public SortOrderDirection SortOrderDirection { get; set; }

        public string SortBy { get; set; }
    }
}
