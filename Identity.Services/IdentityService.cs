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
        private readonly IOptions<JWTSettings> _config;
        public IdentityService(IOptions<JWTSettings> config) 
        {
            _config = config;
        }    
        public TokenGenerationResponse TokenGenerate(TokenGenerationRequest request)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config.Value.Key);

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
                Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(_config.Value.Duration)),
                Issuer = _config.Value.Issuer,
                Audience = _config.Value.Audience,
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
