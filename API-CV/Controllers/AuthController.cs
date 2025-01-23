using Microsoft.AspNetCore.Mvc;
using API_CV.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using API_CV.BdContext;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace API_CV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly string _secretKey;
        private readonly PruebaApiRestContext _context;

        public AuthController(IConfiguration config, PruebaApiRestContext context)
        {
            _secretKey = config["Settings:secretkey"]!;
            _context = context;

        }
        [HttpPost]
        [Route("UserValidation")]
        public async Task<IActionResult> Validation([FromBody] User request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email ==request.Email && u.Password==request.Password);

            if (user == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);

            }
            if (user != null)
            {
                var key = Encoding.ASCII.GetBytes(_secretKey);

                var claims = new ClaimsIdentity();

                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier,user.Email!));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                var tokenhandler = new JwtSecurityTokenHandler();
                var tokenconfig = tokenhandler.CreateToken(tokenDescriptor);

                string tokencreado = tokenhandler.WriteToken(tokenconfig);

                return StatusCode(StatusCodes.Status200OK, new {token = tokencreado});

                }
            return StatusCode(StatusCodes.Status401Unauthorized);


        }
    }


    }

