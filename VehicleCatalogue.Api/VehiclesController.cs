using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VehicleCatalogue.Domain.Models;
using VehicleCatalogue.Services;

namespace VehicleCatalogue.Api
{
    [ApiController]
    [Route("[controller]")]
    public class VehiclesController:ControllerBase
    {
        private readonly IVehicleService _vehicleSearchService;

        public VehiclesController(IVehicleService vehicleSearchService)
        {
            _vehicleSearchService = vehicleSearchService;
        }

        // Search for Vehicles
        [ProducesResponseType(typeof(IEnumerable<VehicleDomainModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("Search")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<VehicleDomainModel>>> Search(VehicleSearchDomainModel searchCriteria)
        {
            var vehicles = await _vehicleSearchService.Search(searchCriteria);

            if (vehicles == null || !vehicles.Any())
                return NotFound();

            return Ok(vehicles);
        }

        // Create Vehicle
        [ProducesResponseType(typeof(VehicleDomainModel), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("")]
        [HttpPost]
        public async Task<ActionResult<IEnumerable<VehicleDomainModel>>> Create(VehicleDomainModel vehicle)
        {
            var vehicles = await _vehicleSearchService.Create(vehicle);

            return Created(string.Empty, vehicle);
        }
    }
}
