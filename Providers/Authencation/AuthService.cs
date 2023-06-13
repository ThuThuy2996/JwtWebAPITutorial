using JwtWebAPITutorial.DTO;
using JwtWebAPITutorial.Interfaces.IAuthencation;
using JwtWebAPITutorial.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace JwtWebAPITutorial.Providers.Authencation
{
    public class AuthService : IAuthService
    {
        private IConfiguration _configuration;
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<ActionResultService> Login(UserDTO userDTO, User user)
        {
            return await CheckLogin(userDTO, user);
        }

        public async Task<ActionResultService> Register(UserDTO user)
        {
            return await HashPassword(user);
        }

        private async Task<ActionResultService> HashPassword(UserDTO userDTO)
        {
            try
            {
                using (var hmac = new HMACSHA512())
                {
                    User user = new User()
                    {
                        UserName = userDTO.UserName,
                        PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(userDTO.Password)),
                        PasswordSalt = hmac.Key
                    };

                    return new ActionResultService()
                    {
                        StatusCode = 200,
                        Success = true,
                        ObjData = user                        
                    };

                }
            }
            catch(Exception e)
            {
                return new ActionResultService()
                {
                    ErrMsg = e.Message,
                    Success = false,
                    ObjData = new User()
                };
            }
        }

        private async Task<ActionResultService> CheckLogin(UserDTO userDTO, User user)
        {
            ActionResultService rs = new ActionResultService();
            try
            {
                if(userDTO.UserName != user.UserName)
                {
                    rs.Success = false;
                    rs.ErrMsg = "Username is not found ! Please check again";
                    rs.StatusCode = 500;
                    return rs;
                }
                if(!VerifyPassword(userDTO.Password, user.PasswordHash, user.PasswordSalt))
                {
                    rs.Success = false;
                    rs.ErrMsg = "Password is wrong ! Please check again";
                    rs.StatusCode = 500;
                    return rs;
                }

                rs.Success = true;
                rs.StatusCode = 200;
                rs.Data = CreateToken(user);
                return rs;
            }
            catch
            {
                return rs;
            }
             
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512(passwordSalt))
            {
                byte[] hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return hash.SequenceEqual(passwordHash);
            }
            
        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSetting:TokenSecrectKey").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);
            string jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
