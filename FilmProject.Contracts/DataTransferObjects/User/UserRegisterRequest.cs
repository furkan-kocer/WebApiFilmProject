namespace FilmProject.Contracts.DataTransferObjects.User
{
    public record UserRegisterRequest(string Email,string Password,string? PhoneNumber);
}
