using Azure.Core;
using Azure.Identity;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Restaurants.Shared.Extensions
{
    public static class FAConfiguration
    {
        public static void Initialize(this IFunctionsHostBuilder builder, IConfiguration Configuration)
        {
            try
            {
                var confBuilder = new ConfigurationBuilder()
                    .AddEnvironmentVariables()
                    .AddConfiguration(Configuration);

                Configuration = confBuilder.Build();

                // this is only used to be able to run in local dev
                var isLocalDev = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID"));

                var (AppConfEndPoint, env, uaiClientId) = GetCoreConfig(isLocalDev);

                // add 2 azure app conf resources to establish region crossover redundancy
                //_ = confBuilder
                //    .AddAzureAppConfiguration(options =>
                //        options.Connect(new Uri(AppConfEndPoint), isLocalDev
                //            ? new DefaultAzureCredential(new DefaultAzureCredentialOptions())
                //            : (TokenCredential)new ManagedIdentityCredential(uaiClientId))
                //        .Select(keyFilter: "General:*", labelFilter: env));

                // To be replaced with code above
                _ = confBuilder.AddAzureAppConfiguration(options => options.Connect("Endpoint=https://restaurant-conf.azconfig.io;Id=5F3U-l9-s0:yA+E1OVww+R0HEtnR5ec;Secret=gB1cQ5vpb1xyMKan00Ml+mC4WUm7c1LMgwwXEULELj0=")
                .Select(keyFilter: "General:*", labelFilter: env));
                //------------------------

                Configuration = confBuilder.Build();

                //AzureServiceTokenProvider azureServiceTokenProvider = isLocalDev
                //    ? new AzureServiceTokenProvider()
                //    : new AzureServiceTokenProvider($"RunAs=App;AppId={uaiClientId}");

                //var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

                //_ = confBuilder.AddAzureKeyVault(Configuration["General:KeyVaultUrl"], keyVaultClient, new DefaultKeyVaultSecretManager());
                //Configuration = confBuilder.Build();

                //confBuilder.AddAzureKeyVault()

                builder.Services.Replace(ServiceDescriptor.Singleton(typeof(IConfiguration), Configuration));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static (string primaryAzAppConfEndPoint, string Environment, string uaiClientId) GetCoreConfig(bool isLocalDev)
        {
            string AzAppConfEndPoint = Environment.GetEnvironmentVariable("AZAPPCONF_ENDPOINT");
            if (string.IsNullOrEmpty(AzAppConfEndPoint)) throw new Exception("AZAPPCONF_ENDPOINT setting is missing, can not start");

            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrEmpty(environment)) throw new Exception("ASPNETCORE_ENVIRONMENT setting is missing, can not start");

            var uaiClientId = Environment.GetEnvironmentVariable("UAI_CLIENTID");
            if (!isLocalDev && string.IsNullOrEmpty(uaiClientId)) throw new Exception("UAI_CLIENTID setting is missing, can not start");

            return (AzAppConfEndPoint, environment, uaiClientId);
        }
    }
}
