namespace Identity.Api.JWTDto
{
    public class TokenGenerationResponse
    {
        public string Token { get; set; }
        public DateTime TokenExpireDate { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
    }
}
