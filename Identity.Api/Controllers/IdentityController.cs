using Identity.Api.JWTDto;
using Identity.Api.Modal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        [HttpPost("token")]
        public IActionResult GenerateToken([FromBody]TokenGenerationRequest request,IOptions<JWTSettings> config)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(config.Value.Key);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, request.phoneNumber == null ? request.email : request.phoneNumber),
                new(JwtRegisteredClaimNames.Email, request.email),
                new("userid", request.userID.ToString()),
                new("role",request.Role.ToString()),
                request.phoneNumber !=null ? new("Phone Number", request.phoneNumber) : null
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TimeSpan.FromHours(config.Value.Duration)),
                Issuer = config.Value.Issuer,
                Audience = config.Value.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var response = tokenHandler.WriteToken(token);
            return Ok(new TokenGenerationResponse
            {
                Token = response,
                TokenExpireDate = tokenDescriptor.Expires.Value,
                Email = request.email,
                UserId = request.userID
            });
        }
    }
}
