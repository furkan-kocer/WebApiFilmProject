namespace FilmProject.Contracts.DataTransferObjects.User
{
    public record UserLoginResponse(
        string Token,
        string? RefreshToken,
        DateTime TokenExpireDate,
        string Email,
        string UserId);
}
