
using CarRentalSystem.Models;
using CarRentalSystem.Data;
using CarRentalSystem.Repositories;
using CarRentalSystem.Utilities;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Services
{
    public class UserService:IUserService
    {
        //private readonly IUserRepository _userRepository;
        private readonly AppDbContext context;

        public UserService(AppDbContext context)
        {
           this. context = context;
        }

        public bool RegisterUser(User user)
        {
            // Check if email already exists
            if (context.Users.Any(u=>u.Email == user.Email))
                return false;
            user.Role = string.IsNullOrEmpty(user.Role) ? "User" : user.Role;

            // Save the user
            context.Users.Add(user);
            context.SaveChanges();
            return true;
        }

        public string AuthenticateUser(string email, string password)
        {
            // Check if the user exists and password matches
            var user = context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            //var user = _userRepository.GetUserByEmail(email);
            if (user != null && user.Password == password)
            {
                string role = user.Role;
                // Generate and return a JWT token
                return JwtHelper.GenerateToken(user.Email, role, "HelloIamCharmeePradhyumnaIamDoingC#assignmentThisWeekIamlearninganddoingthisassignment");
            }

            return null; // Authentication failed
        }

    }
}
