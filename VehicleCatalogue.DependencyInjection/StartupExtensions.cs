using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VehicleCatalogue.Services;

namespace VehicleCatalogue.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class StartupExtensions
    {
        // <summary>
        /// Add Service Providers
        /// </summary>
        /// <param name="serviceCollection">Service Collection</param>
        /// <param name="configuration">Configuration</param>
        /// <returns>Binded Services Collection</returns>
        public static IServiceCollection AddServiceProviders(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddTransient<IVehicleService, VehicleService>();
            return serviceCollection;
        }
    }
}
