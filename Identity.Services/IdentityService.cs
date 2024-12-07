using Identity.Domain.JWTDto;
using Identity.Domain.Modal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Services
{
    public class IdentityService : _IdentityService
    {
        public TokenGenerationResponse TokenGenerate(TokenGenerationRequest request, IOptions<JWTSettings> config)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(config.Value.Key);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, string.IsNullOrEmpty(request.phoneNumber) ? request.email : request.phoneNumber),
                new(JwtRegisteredClaimNames.Email, request.email),
                new("userid", request.userID.ToString()),
                new("role",request.Role.ToString()),
                string.IsNullOrEmpty(request.phoneNumber) ? null : new("Phone Number", request.phoneNumber)
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TimeSpan.FromHours(config.Value.Duration)),
                Issuer = config.Value.Issuer,
                Audience = config.Value.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token =  tokenHandler.CreateToken(tokenDescriptor);
            var response = tokenHandler.WriteToken(token);
            return new TokenGenerationResponse
            {
                Token = response,
                TokenExpireDate = tokenDescriptor.Expires.Value,
                Email = request.email,
                UserId = request.userID
            };
        }
    }
}
