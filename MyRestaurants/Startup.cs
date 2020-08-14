using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRestaurants.Services;
using Restaurants.Shared.Extensions;
using Restaurants.Shared.Models;
using Restaurants.Shared.Repositories;

[assembly: FunctionsStartup(typeof(MyRestaurants.Startup))]
namespace MyRestaurants
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Initialize(builder.Services.BuildServiceProvider().GetService<IConfiguration>());
            InitializeServices(builder.Services);
        }

        private void InitializeServices(IServiceCollection services)
        {
            var conf = services.BuildServiceProvider().GetService<IConfiguration>();
            services.Configure<JWTSettings>(conf.GetSection($"SmpGeneral:{nameof(JWTSettings)}"));
            services.Configure<DbSettings>(conf.GetSection($"SmpGeneral:{nameof(DbSettings)}"));
            services.AddTransient<IRestaurantService, RestaurantService>();
            services.AddSingleton<IRestaurantRepository, RestaurantRepository>();
        }
    }
}
