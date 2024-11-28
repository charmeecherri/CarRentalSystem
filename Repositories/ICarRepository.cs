using CarRentalSystem.Models;
using System.Collections.Generic;


namespace CarRentalSystem.Repositories
{
    public interface ICarRepository
    {
      
            void AddCar(Car car);
            Car GetCarById(int id);
            IEnumerable<Car> GetAvailableCars();
            void UpdateCarAvailability(int id, bool isAvailable);
    }


    
}
