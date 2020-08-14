using Restaurants.Shared.Models;
using Restaurants.Shared.Repositories;
using System.Threading.Tasks;

namespace MyAccount.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRestaurantRepository RestaurantRepository;

        public AccountService(IRestaurantRepository restaurantRepository)
        {
            RestaurantRepository = restaurantRepository;
        }

        public Task<User> CreateAccount(User user) => RestaurantRepository.AddAccount(user);

        public Task<User> Login(User user) => RestaurantRepository.Login(user);
    }
}
