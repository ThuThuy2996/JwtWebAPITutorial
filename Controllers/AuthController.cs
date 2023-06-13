using JwtWebAPITutorial.DTO;
using JwtWebAPITutorial.Interfaces.IAuthencation;
using JwtWebAPITutorial.Models;
using JwtWebAPITutorial.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtWebAPITutorial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _registerService;
        private static User _user = new User();

        public AuthController(IAuthService registerService)
        {
            _registerService = registerService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResultService> Register([FromBody] UserDTO user)
        {
            var rs = await _registerService.Register(user);
            _user = (User)rs.ObjData;
            return rs;
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResultService> Login([FromBody] UserDTO user)
        {
            return await _registerService.Login(user, _user);
        }
    }
}
