using CarRentalSystem.Models;

namespace CarRentalSystem.Services
{
    public interface IUserService
    {
        bool RegisterUser(User user);
        string AuthenticateUser(string email, string password);
    }
}
