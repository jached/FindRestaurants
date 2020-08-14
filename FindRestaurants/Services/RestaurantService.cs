using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Restaurants.Shared.Models;

namespace FindRestaurants.Service
{
    public class RestaurantService : IRestaurantService
    {
        private readonly ILogger Logger;
        private readonly IConfiguration Configuration;
        private readonly IHttpClientFactory HttpClientFactory;

        public RestaurantService(ILogger logger, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            Logger = logger;
            Configuration = configuration;
            HttpClientFactory = httpClientFactory;
        }

        public async Task<List<Restaurant>> GetNearbyRestaurants(Area area)
        {
            var httpClient = HttpClientFactory.CreateClient("FindRestaurantsSender");
            //var key = Configuration["GoogleApiKey"];
            var key = "AIzaSyD9HJFN5mL1VnFRTvJo-au6k3xiCI0gJlM";
            var radius = area.Radius;
            var latitude = area.Position.Latitude;
            var longitude = area.Position.Longitude;
            var requestUri = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={latitude},{longitude}&radius={radius}&type=restaurant&key={key}";
            var response = await httpClient.GetAsync(requestUri).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return GetRelevantInformation(json);
        }

        private List<Restaurant> GetRelevantInformation(string json)
        {
            var googleRestaurants = JsonConvert.DeserializeObject<GoogleResult>(json).Results;
            return googleRestaurants.Select(gr => new Restaurant()
            {
                Name = gr.Name,
                Address = gr.Vicinity,
                Position = new Position { Latitude = gr.Geometry.Lat, Longitude = gr.Geometry.Lng },
                Id = gr.Id
            }).ToList();
        }
    }
}
