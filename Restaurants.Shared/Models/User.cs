using Newtonsoft.Json;
using System.Collections.Generic;

namespace Restaurants.Shared.Models
{
    public class User
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("restaurants")]
        public List<Restaurant> Restaurants { get; set; }
    }
}
