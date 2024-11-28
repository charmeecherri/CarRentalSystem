using CarRentalSystem.Models;
using CarRentalSystem.Repositories;
using CarRentalSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
            private readonly ICarRentalService _carRentalService;
            private readonly ICarRepository _carRepository;

            public CarController(ICarRentalService carRentalService, ICarRepository carRepository)
            {
                _carRentalService = carRentalService;
                _carRepository = carRepository;
            }


        [HttpGet("Available")]
       public IActionResult GetAvailableCars()
        {
           var cars = _carRepository.GetAvailableCars();
          return Ok(cars);
        }

      

        [HttpGet("CheckAvailability/{carId}")]
        public IActionResult CheckCarAvailability(int carId)
        {
            var isAvailable = _carRentalService.CheckCarAvailability(carId);
            return Ok(new { IsAvailable = isAvailable });
        }


        [HttpPost("RentCar")]
        public IActionResult RentCar(int carId, int userId)
        {
            if (_carRentalService.RentCar(carId, userId))
                return Ok("Car rented successfully!");
            return BadRequest("Car is unavailable or doesn't exist.");
        }



        // Only Admins can access

        [Authorize(Roles = "Admin")]  
        [HttpPost("AddCar")]
        public IActionResult AddCar( Car car)
        {
            _carRepository.AddCar(car);
            return CreatedAtAction(nameof(GetAvailableCars), new { id = car.Id }, car);
        }

        
        [Authorize(Roles = "User,Admin")]  
        [HttpPut("{id}")]
        public IActionResult UpdateCarAvailability(int id, Car car)
        {
            _carRepository.UpdateCarAvailability(id, car.IsAvailable);
            return NoContent();
        }

        
        [Authorize(Roles = "Admin")]  
        [HttpDelete("{id}")]
        public IActionResult DeleteCar(int id)
        {
            var car = _carRepository.GetCarById(id);
            if (car == null) return NotFound();
            _carRepository.GetAvailableCars().ToList().Remove(car);  
            return NoContent();
        }
    }
}
