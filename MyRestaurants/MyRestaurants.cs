using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.VisualBasic.CompilerServices;
using Restaurants.Shared.Extensions;
using Restaurants.Shared.Models;
using Restaurants.Shared;
using System.Collections.Generic;
using MyRestaurants.Services;

namespace MyRestaurants
{
    public class MyRestaurants
    {
        private readonly JWTHandler JWTHandler;
        private readonly IRestaurantService RestaurantService;

        public MyRestaurants(JWTHandler jwtHandler, IRestaurantService restaurantService)
        {
            JWTHandler = jwtHandler;
            RestaurantService = restaurantService;
        }

        [FunctionName("GetMyRestaurants")]
        public async Task<IActionResult> GetMyRestaurants(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            bool isAuthorized;
            string username;
            string userId;
            try
            {
                (isAuthorized, username, userId) = JWTHandler.CheckJWT(req.HttpContext.Request.Headers);
            }
            catch (Exception e)
            {
                log.LogWarning("Failed to validate token", e);
                throw;
            }

            if (!isAuthorized)
                return new UnauthorizedResult();

            try
            {
                var restaurants = await RestaurantService.GetMyRestaurants(username, userId);
                return new OkObjectResult(restaurants);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [FunctionName("SaveRestaurant")]
        public async Task<IActionResult> SaveRestaurant(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            bool isAuthorized;
            string username;
            string userId;
            try
            {
                (isAuthorized, username, userId) = JWTHandler.CheckJWT(req.HttpContext.Request.Headers);
            }
            catch (Exception e)
            {
                log.LogWarning("Failed to validate token", e);
                throw;
            }

            if (!isAuthorized)
                return new UnauthorizedResult();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Restaurant restaurant = JsonConvert.DeserializeObject<Restaurant>(requestBody);

            try
            {
                await RestaurantService.SaveRestaurant(restaurant, username, userId);
                return new OkResult();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [FunctionName("RemoveRestaurant")]
        public async Task<IActionResult> RemoveRestaurant(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            bool isAuthorized;
            string username;
            string userId;
            try
            {
                (isAuthorized, username, userId) = JWTHandler.CheckJWT(req.HttpContext.Request.Headers);
            }
            catch (Exception e)
            {
                log.LogWarning("Failed to validate token", e);
                throw;
            }

            if (!isAuthorized)
                return new UnauthorizedResult();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Restaurant restaurant = JsonConvert.DeserializeObject<Restaurant>(requestBody);

            try
            {
                await RestaurantService.RemoveSavedRestaurant(restaurant, username, userId);
                return new OkResult();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
