using Restaurants.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyRestaurants.Services
{
    public interface IRestaurantService
    {
        Task<List<Restaurant>> GetMyRestaurants(string username, string userId);
        Task SaveRestaurant(Restaurant restaurant, string username, string userId);
        Task RemoveSavedRestaurant(Restaurant restaurant, string username, string userId);
    }
}
