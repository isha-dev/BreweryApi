using BreweryApi.Models;
using BreweryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BreweryApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenService _jwtService;

        public AuthController(JwtTokenService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Username == "admin" && request.Password == "admin123")
            {
                var token = _jwtService.GenerateToken(request.Username);
                return Ok(new { Token = token });
            }
            return Unauthorized("Invalid credentials");
        }
    }


}
