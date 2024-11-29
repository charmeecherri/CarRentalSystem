using CarRentalSystem.Models;
using CarRentalSystem.Repositories;
using CarRentalSystem.Services;
using CarRentalSystem.Utilities;
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
        private readonly IUserService _userService;
        private readonly ICarRepository _carRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        //private ICarRentalService carRentalService;

        public CarController(ICarRentalService carRentalService, ICarRepository carRepository,IUserRepository userRepository, IEmailService emailService)
            {
                _carRentalService = carRentalService;
                _carRepository = carRepository;
                _userRepository = userRepository;
                _emailService = emailService;

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
        public async Task<IActionResult> RentCarAsync(int carId, int userId)
        {

            if (_carRentalService.RentCar(carId, userId))
            {
                var user = _userRepository.GetUserById(userId);

                if (user != null)
                {
                    var mail_body = $"Hi {user.Name}" +
                       "<br>" +
                       "Thank you for contacting Chubb. We appreciate your interest in our services." +
                       "<br>" +
                       "<br>" +
                       "Thanks & Regards" +
                       "<br>" +
                       "Car Rentail Service";
                    var message = new Message([user.Email], "Car Rental Services Request", mail_body, null);
                    await _emailService.SendEmailAsync(message);
                    return Ok("mail sent successfully");
                }
                
                return BadRequest("User not found");

            }

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

        
        [Authorize(Roles = "user,Admin")]  
        [HttpPut("{id}")]
        public IActionResult UpdateCarAvailability(int id, bool isAvailable)
        {
            _carRepository.UpdateCarAvailability(id, isAvailable);
            return NoContent();
        }


        [Authorize(Roles = "user,Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteCar(int id)
        {
            var car = _carRepository.GetCarById(id);
            if (car == null) return NotFound();
            _carRepository.DeleteCar(car);
            return NoContent();
        }
    }
}
