using Restaurants.Shared.Models;
using System.Threading.Tasks;

namespace MyAccount.Services
{
    public interface IAccountService
    {
        Task<User> Login(User user);
        Task<User> CreateAccount(User user);
    }
}
