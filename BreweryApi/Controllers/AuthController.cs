using BreweryApi.Models;
using BreweryApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;

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

        [HttpGet("login")]
        public IActionResult Login()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return Unauthorized("Missing Authorization header");
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

            var credentialBytes = Convert.FromBase64String(authHeader.Parameter ?? "");
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
            if (credentials.Length != 2)
                return Unauthorized("Invalid credentials");

            var username = credentials[0];
            var password = credentials[1];

            if (username == "admin" && password == "admin123")
            {
                var token = _jwtService.GenerateToken(username);
                return Ok(new { Token = token });
            }
            else
                return Unauthorized("Invalid credentials");
        }
    }


}
