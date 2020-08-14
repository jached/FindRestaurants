using System.Collections.Generic;
using System.Threading.Tasks;
using Restaurants.Shared.Models;

namespace FindRestaurants.Service
{
    public interface IRestaurantService
    {
        Task<List<Restaurant>> GetNearbyRestaurants(Area area);
    }
}
