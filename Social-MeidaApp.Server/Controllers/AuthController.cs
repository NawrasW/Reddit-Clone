using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Social_MeidaApp.Server.Models.Dto;
using Social_MeidaApp.Server.Repsitories.Abstrsct;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Social_MeidaApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {


        private readonly IAuthRepsitory _authRepsitory;
        private readonly IConfiguration _configuration;
        public AuthController(IAuthRepsitory authRepsitory, IConfiguration configuration)
        {
            _authRepsitory = authRepsitory;
            _configuration = configuration;
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login(UserForLoginDto user)
        {

            var result = await _authRepsitory.Login(user);
            if (result == null)
                return new UnauthorizedResult();

            //create jwt
            result.Token = CreateJWT(result);
            return Ok(result);
        }

        private string CreateJWT(UserLoginDto user)
        {
            var secretKey = _configuration.GetSection("AppSettings:key").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(secretKey));


            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Name ),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role??""),
            };

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(15),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }


        [HttpPost("register")]

        public async Task<IActionResult> Register(UserDto user)
        {
            if (await _authRepsitory.UserAlreadyExists(user.Email))
                return BadRequest("User Already Exists, Please try a different email...!");
            var result = await _authRepsitory.Register(user);
            var status = new Status
            {

                StatusCode = result ? 1 : 0,
                StatusMessage = result ? "Added user Successfully" : "Error Adding a user......."


            };
            return Ok(status);

        }







    }
}
