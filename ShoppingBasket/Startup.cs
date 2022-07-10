using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShoppingBasket.Application.Services.Implementations;
using ShoppingBasket.Application.Services.Interfaces;
using ShoppingBasket.Data.Gateways.Implementations;
using ShoppingBasket.Data.Gateways.Interfaces;
using ShoppingBasket.Data.Repositories.Implementations;
using ShoppingBasket.Data.Repositories.Interfaces;

namespace ShoppingBasket.Presentation
{
    internal static class Startup
    {
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.SetBasePath(Directory.GetCurrentDirectory());
                })
                .ConfigureServices((context, services) =>
                {
                    RegisterRepositories(services);
                    RegisterGateways(services);
                    RegisterServices(services);
                });

            return hostBuilder;
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddSingleton<IItemsRepository, ItemsRepository>();
        }

        private static void RegisterGateways(IServiceCollection services)
        {
            services.AddSingleton<IPromotionServiceGateway, PromotionServiceGateway>();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IShoppingBasketService, ShoppingBasketService>();
        }
    }
}