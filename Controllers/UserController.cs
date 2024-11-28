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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICarRepository _carRepository;

        public UserController(IUserService userService,ICarRepository carRepository)
        {
            _userService = userService;
            _carRepository = carRepository;
        }

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            if (_userService.RegisterUser(user))
                return Ok("User registered successfully!");
            return BadRequest("Email already exists.");
        }

        [HttpPost("login")]
        public IActionResult Login([FromQuery] string email, string password)
        {
            var token = _userService.AuthenticateUser(email, password);
            if (token != null)
                return Ok(new { Token = token });
            return Unauthorized("Invalid email or password.");
        }
        [Authorize(Roles = "User")]
        [HttpGet("getCars")]
        public IActionResult GetCars()
        {
            
            var cars = _carRepository.GetAvailableCars();
            return Ok(cars);
        }

    }
}
