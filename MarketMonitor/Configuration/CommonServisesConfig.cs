using MarketMonitor.Core.Contracts.Services;
using MarketMonitor.Core.Services;
using MarketMonitor.DataProvider.Fintacharts;
using MarketMonitor.DataProviders.Fintacharts;
using MarketMonitor.DataStore.SQL.Repositories;
using MarketMonitor.RI.Contracts.Clients;
using MarketMonitor.RI.Contracts.Repositories;

namespace MarketMonitor.API
{
    public static class CommonConfigurations
    {
        public static IServiceCollection AddServicesConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IAssetService, AssetService>();
            return services;
        }

        public static IServiceCollection AddProvidersConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IAssetRepo, AssetRepo>();
            services.AddScoped<IPriceRepo, PriceRepo>();
            services.AddScoped<IFintachartsWebSocketClient, FintachartsWSClient>();

            return services;
        }

        public static IServiceCollection AddClientsConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IFintachartsHttpClient, FintachartsHttpClient>();

            return services;
        }
    }
}
