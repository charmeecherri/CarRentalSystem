using CarRentalSystem.Data;
using CarRentalSystem.Models;
using CarRentalSystem.Repositories;

namespace CarRentalSystem.Services
{
    public class CarRentalService:ICarRentalService
    {
        //private readonly ICarRepository _carRepository;
        //private readonly IUserRepository _userRepository;
        private readonly AppDbContext context;
       

        public CarRentalService(AppDbContext context)
        {
            this.context=context;
        }

        public bool RentCar(int carId, int userId)
        {
            var car = context.Cars.FirstOrDefault(c => c.Id == carId);
            if (car != null && car.IsAvailable)
            {
                
                car.IsAvailable=false;
                context.SaveChanges();
                return true;
            }
            return false; // if Car is unavailable or doesn't exist
        }

        public bool CheckCarAvailability(int carId)
        {
            var car = context.Cars.FirstOrDefault(c => c.Id == carId);
            return car?.IsAvailable ?? false; // Return false if car is null
        }

    }
}
