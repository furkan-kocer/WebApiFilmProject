using Identity.Domain.JWTDto;
using Identity.Domain.Modal;
using Identity.Services.ExternalApi;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
        public TokenGenerationResponse CreateToken(TokenGenerationRequest request)
        {
            var response = HandleTokenTransactions(request);
            var refreshToken = GenerateRefreshToken();
            return new TokenGenerationResponse
            {
                Token = response,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = DateTime.UtcNow.Add(TimeSpan.FromMinutes(_config.Value.RefreshTokenExpiryDate))
            };
        }
        private NewTokenDto CreateNewTokens(TokenGenerationRequest request)
        {
            var response = HandleTokenTransactions(request);
            var refreshToken = GenerateRefreshToken();
            return new NewTokenDto(Token: response, refreshToken);
        }
        private string HandleTokenTransactions(TokenGenerationRequest request)
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
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var response = tokenHandler.WriteToken(token);
            return response;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                Convert.ToBase64String(randomNumber);
            }
            var token = $"{Convert.ToBase64String(randomNumber)}-{DateTime.UtcNow.Ticks}";
            return token;
        }
        private TokenValidationResultDTO ValidateToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Value.Key)),
                ValidateLifetime = false,
                ValidIssuer = _config.Value.Issuer,
                ValidAudience = _config.Value.Audience,
                ClockSkew = TimeSpan.Zero
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg
                .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return new TokenValidationResultDTO
                {
                    IsValid = false,
                    IsExpired = false,
                    Message = "Invalid token"
                };
            }
            // Check expiration manually
            var expiryDateUnix = long.Parse(jwtSecurityToken.Claims.First(x => x.Type == "exp").Value);
            var expiryDate = DateTimeOffset.FromUnixTimeSeconds(expiryDateUnix).UtcDateTime;
            if (expiryDate < DateTime.UtcNow)
            {
                return new TokenValidationResultDTO
                {
                    IsValid = true,
                    IsExpired = true,
                    Message = "Token is expired"
                };
            }
            return new TokenValidationResultDTO
            {
                IsValid = true,
                IsExpired = false,
                Message = "Token is not expired."
            };
        }
        public GenericResponseBase<NewTokenDto> RefreshTokens(RefreshTokenGenerationRequest request)
        {
            if (request.Token == null)
            {
                return GenericResponseBase<NewTokenDto>.Error("Token can not be null.");
            }
            var tokenValidationResult = ValidateToken(request.Token);
            if (!tokenValidationResult.IsValid)
            {
                return GenericResponseBase<NewTokenDto>.Error(tokenValidationResult.Message);
            }
            var createToken = new TokenGenerationRequest
            {
                userID = request.userID,
                email = request.email,
                phoneNumber = request.phoneNumber,
                Role = request.Role
            };
            var createNewToken = CreateNewTokens(createToken);
            return GenericResponseBase<NewTokenDto>.Success(createNewToken);
        }
    }
}
