using System;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VehicleCatalogue.CosmosDataprovider.Interfaces;
using VehicleCatalogue.CosmosDataprovider.Providers;
using VehicleCatalogue.Domain.Interfaces;

namespace VehicleCatalogue.CosmosDataprovider
{
    public static class StartupExtensions
    {
        /// <summary>
        /// Add Providers
        /// </summary>
        /// <param name="serviceCollection">Service Collection</param>
        /// <param name="configuration">Configuration</param>
        /// <returns>Binded Services Collection</returns>
        public static IServiceCollection AddDataProviders(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddTransient<IVehiclesDataProvider, VehiclesDataProvider>();
            serviceCollection.AddTransient<ICosmosDbConnector, CosmosDbConnector>();
            serviceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return serviceCollection;
        }
    }
}
