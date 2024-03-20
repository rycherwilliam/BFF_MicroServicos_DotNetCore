using BFFAPI.Domain.Models;
using BFFAPI.Services.Autenticacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BFFAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[Action]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtAuthService _jwtAuthService;

        public AuthController(IJwtAuthService jwtAuthService)
        {
            _jwtAuthService = jwtAuthService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login model)
        {
            var token = _jwtAuthService.GenerateToken("0", "Will");
            return Ok(new { Token = token });
        }
    }
}
