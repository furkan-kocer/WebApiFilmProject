namespace Identity.Api.JWTDto
{
    public class TokenGenerationResponse
    {
        public string Token { get; set; }
        public DateTime TokenExpireDate { get; set; }
    }
}
