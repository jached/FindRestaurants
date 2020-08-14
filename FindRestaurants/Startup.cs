using FindRestaurants.Service;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Restaurants.Shared.Extensions;
using System;
using System.Net.Http;

[assembly: FunctionsStartup(typeof(FindRestaurants.Startup))]
namespace FindRestaurants
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
            services.AddTransient<IRestaurantService, RestaurantService>();
            services.AddHttpClient("FindRestaurantsSender")
                .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10)))
                .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder.CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 2,
            durationOfBreak: TimeSpan.FromMinutes(1)
    ));
        }
    }
}
