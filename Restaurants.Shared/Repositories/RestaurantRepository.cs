using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Restaurants.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurants.Shared.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly IOptions<DbSettings> DbSettings;
        private readonly IConfiguration Configuration;
        private readonly ILogger Logger;
        private readonly CosmosClient CosmosClient;

        public RestaurantRepository(IOptions<DbSettings> dbSettings, IConfiguration configuration, ILogger logger)
        {
            DbSettings = dbSettings;
            Configuration = configuration;
            Logger = logger;
            //CosmosClient = new CosmosClientBuilder(DbSettings.Value.EndPoint, Configuration["DbSettings:AuthKey"]).Build();
            var key = "yCGSsNfBT6o0TBKpYSMKVm1xeCG1l5mEvuHeNr7vgPYAy9C2MiC7Ic2CfSzRXyuLSMrQQdkw5YtYHCAkR5Zvfw==";
            CosmosClient = new CosmosClientBuilder(DbSettings.Value.EndPoint, key).Build();
        }

        public async Task<Models.User> AddAccount(Models.User user)
        {
            try
            {
                Container container = CosmosClient.GetContainer(DbSettings.Value.DbName, DbSettings.Value.ContainerName);
                user.Id = Guid.NewGuid().ToString();
                _ = await container.CreateItemAsync(user).ConfigureAwait(false);
                return user;
            }
            catch (Exception e)
            {
                Logger.LogError("Add account Cosmos error", e);
                throw;
            }
        }

        public async Task<Models.User> Login(Models.User user)
        {
            try
            {
                Container container = CosmosClient.GetContainer(DbSettings.Value.DbName, DbSettings.Value.ContainerName);
                using (FeedIterator<Models.User> setIterator = container.GetItemLinqQueryable<Models.User>()
                    .Where(u => u.Username == user.Username && u.Password == user.Password)
                    .ToFeedIterator())
                {
                    var respons = await setIterator.ReadNextAsync();
                    return respons.First();
                }
            }
            catch (Exception e)
            {
                Logger.LogError("Login user Cosmos error", e);
                throw;
            }
        }

        public async Task<List<Restaurant>> GetRestaurants(string username, string userId)
        {
            try
            {
                Container container = CosmosClient.GetContainer(DbSettings.Value.DbName, DbSettings.Value.ContainerName);
                Models.User userFromCosmos = await container.ReadItemAsync<Models.User>(userId, new PartitionKey(username)).ConfigureAwait(false);
                return userFromCosmos.Restaurants;
            }
            catch (Exception e)
            {
                Logger.LogError("Get restaurants Cosmos error", e);
                throw;
            }
        }

        public async Task SaveRestaurant(Restaurant restaurant, string username, string userId)
        {
            try
            {
                Container container = CosmosClient.GetContainer(DbSettings.Value.DbName, DbSettings.Value.ContainerName);
                Models.User userFromCosmos = await container.ReadItemAsync<Models.User>(userId, new PartitionKey(username)).ConfigureAwait(false);
                userFromCosmos.Restaurants.Add(restaurant);
                _ = await container.UpsertItemAsync(userFromCosmos).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logger.LogError("Save restaurant Cosmos error", e);
                throw;
            }
        }

        public async Task RemoveSavedRestaurant(Restaurant restaurant, string username, string userId)
        {
            try
            {
                Container container = CosmosClient.GetContainer(DbSettings.Value.DbName, DbSettings.Value.ContainerName);
                Models.User userFromCosmos = await container.ReadItemAsync<Models.User>(userId, new PartitionKey(username)).ConfigureAwait(false);
                userFromCosmos.Restaurants.Remove(restaurant);
                _ = await container.UpsertItemAsync(userFromCosmos).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logger.LogError("Remove restaurant Cosmos error", e);
                throw;
            }
        }
    }
}
