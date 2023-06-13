using JwtWebAPITutorial.DTO;
using JwtWebAPITutorial.Models;
using JwtWebAPITutorial.Providers;

namespace JwtWebAPITutorial.Interfaces.IAuthencation
{
    public interface IAuthService
    {
       public Task<ActionResultService> Register(UserDTO userDTO);

        public Task<ActionResultService> Login(UserDTO userDTO, User user);

    }
}
