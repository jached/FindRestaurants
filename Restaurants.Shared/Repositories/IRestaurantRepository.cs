using Restaurants.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Shared.Repositories
{
    public interface IRestaurantRepository
    {
        Task<User> Login(User user);
        Task<User> AddAccount(User user);
        Task SaveRestaurant(Restaurant restaurant, string username, string userId);
        Task RemoveSavedRestaurant(Restaurant restaurant, string username, string userId);
        Task<List<Restaurant>> GetRestaurants(string username, string userId);
    }
}
