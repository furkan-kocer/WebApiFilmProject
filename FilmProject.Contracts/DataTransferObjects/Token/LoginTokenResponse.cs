namespace FilmProject.Contracts.DataTransferObjects.Token
{
    public record LoginTokenResponse(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);
}
