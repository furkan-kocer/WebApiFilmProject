namespace Identity.Domain.Modal
{
    public class JWTSettings
    {
     
        public string Issuer { get; init; }
        public string Audience { get; init; }
        public string Key { get; init; }
        public double Duration { get; init; }
        public double RefreshTokenExpiryDate { get; init; }
    }
}
