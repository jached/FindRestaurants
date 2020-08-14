using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Restaurants.Shared.Models;
using FindRestaurants.Service;
using System.Collections.Generic;

namespace FindRestaurants
{
    public class GetRestaurants
    {
        public IRestaurantService RestaurantService { get; }

        public GetRestaurants(IRestaurantService restaurantService)
        {
            RestaurantService = restaurantService;
        }

        [FunctionName("GetRestaurants")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Area area = JsonConvert.DeserializeObject<Area>(requestBody);

            List<Restaurant> restaurants = await RestaurantService.GetNearbyRestaurants(area);

            return new OkObjectResult(restaurants);
        }
    }
}
