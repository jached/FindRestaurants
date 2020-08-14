using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRestaurants.Services;
using Restaurants.Shared.Extensions;
using Restaurants.Shared.Repositories;
using Restaurants.Shared.Settings;

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
            services.Configure<JWTSettings>(conf.GetSection($"General:{nameof(JWTSettings)}"));
            services.Configure<DbSettings>(conf.GetSection($"General:{nameof(DbSettings)}"));
            services.AddTransient<IRestaurantService, RestaurantService>();
            services.AddSingleton<IRestaurantRepository, RestaurantRepository>();
        }
    }
}
