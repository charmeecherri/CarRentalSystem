using CarRentalSystem.Data;
using CarRentalSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Repositories
{
    public class CarRepository:ICarRepository
    {
        private readonly AppDbContext context;

        public CarRepository(AppDbContext context)
        {
            this.context = context;
        }

        public void AddCar(Car car)
        {
            context.Cars.Add(car);  
            context.SaveChanges();
        }

        public Car GetCarById(int id)
        {
            return context.Cars.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Car> GetAvailableCars()
        {
            return context.Cars.Where(c => c.IsAvailable);
        }

        public void UpdateCarAvailability(int id, bool isAvailable)
        {
            var car = GetCarById(id);
            if (car != null)
            {
                car.IsAvailable = isAvailable;
                context.SaveChanges();

            }
        }

        public void DeleteCar(Car car)
        {
            try
            {
                context.Cars.Remove(car);
                context.SaveChanges();
            }
            catch
            {
                throw;
            }
        }


    }
}
