namespace Identity.Domain.JWTDto
{
    public class TokenValidationResultDTO
    {
        public bool IsValid { get; set; }
        public bool IsExpired { get; set; }
        public string Message { get; set; }
    }
}
