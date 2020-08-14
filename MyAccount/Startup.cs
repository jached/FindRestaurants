using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyAccount.Services;
using Restaurants.Shared;
using Restaurants.Shared.Extensions;
using Restaurants.Shared.Models;
using Restaurants.Shared.Repositories;

[assembly: FunctionsStartup(typeof(MyAccount.Startup))]
namespace MyAccount
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
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<JWTHandler>();
            services.AddSingleton<IRestaurantRepository, RestaurantRepository>();
        }
    }
}
