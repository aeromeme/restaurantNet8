using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using restaurantAPI.Application.DTOs;
using restaurantAPI.Application.Services;

namespace restaurantAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token if credentials are valid.
        /// </summary>
        /// <param name="loginDto">The login credentials.</param>
        /// <returns>
        /// 200 OK: <c>{ "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." }</c><br/>
        /// 401 Unauthorized: If credentials are invalid.
        /// </returns>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            // Validate user credentials here (pseudo-code)
            if (loginDto.Username == "admin" && loginDto.Password == "password")
            {
                var token = _jwtService.GenerateToken(loginDto.Username, "Admin");
                return Ok(new TokenResponse { Token = token });
            }
            return Unauthorized();
        }
    }

    /// <summary>
    /// Represents the JWT token response.
    /// </summary>
    public class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}
