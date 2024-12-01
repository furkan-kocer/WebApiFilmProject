namespace FilmProject.DataAccess.DataTransferObjects.User
{
    public record UserRegisterRequest(string Email,string Password,string? PhoneNumber);
}
