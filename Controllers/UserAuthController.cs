using JWTTestproj.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTTestproj.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserAuthController : ControllerBase
    {

        public readonly JWTContext _jwtContext;
        public readonly JWTSettings  _jwtSettings;

        public UserAuthController(JWTContext jwtContext, IOptions<JWTSettings> jwtSettings)
        {
            _jwtContext = jwtContext;   
             _jwtSettings = jwtSettings.Value;
        }



        [HttpGet ,Route("GetALlUsers")]

        public IActionResult GetAllUsers()
        {
            return Ok(_jwtContext.Users.ToList());
        }

        [AllowAnonymous]
        [HttpPost,Route("Authenticate")]
        public IActionResult Authentication([FromBody] UserModel userModel)
        {
            User_Table User = _jwtContext.Users.FirstOrDefault(x => x.UserName.Trim().ToLower() == userModel.UserName.Trim().ToLower());
            if (User == null)
            {
                return Unauthorized(userModel);
            }
            else
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                var tokenKey = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
                var tokenDecriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity
                        (
                            new Claim[]
                            {
                                new Claim(ClaimTypes.Name,User.UserID.ToString())
                            }
                        ),
                    Expires = DateTime.Now.AddSeconds(60),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
                };
                var OToken = tokenHandler.CreateToken(tokenDecriptor);
                var finalToken = tokenHandler.WriteToken(OToken);
                return Ok(finalToken);
            }

        }
    }
}
