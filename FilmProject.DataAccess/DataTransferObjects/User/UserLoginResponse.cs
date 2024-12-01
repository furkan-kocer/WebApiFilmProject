namespace FilmProject.DataAccess.DataTransferObjects.User
{
    public record UserLoginResponse(
        string Token,
        string RefreshToken,
        DateTime TokenExpireDate,
        string UserName,
        string Email,
        string Role,
        Guid UserId);
}
