using Restaurants.Shared.Models;
using Restaurants.Shared.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyRestaurants.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository RestaurantRepository;

        public RestaurantService(IRestaurantRepository restaurantRepository)
        {
            RestaurantRepository = restaurantRepository;
        }

        public Task<List<Restaurant>> GetMyRestaurants(string username, string userId) => RestaurantRepository.GetRestaurants(username, userId);

        public Task RemoveSavedRestaurant(Restaurant restaurant, string username, string userId) => RestaurantRepository.RemoveSavedRestaurant(restaurant, username, userId);

        public Task SaveRestaurant(Restaurant restaurant, string username, string userId) => RestaurantRepository.SaveRestaurant(restaurant, username, userId);
    }
}
