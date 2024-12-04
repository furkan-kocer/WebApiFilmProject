namespace FilmProject.DataAccess.Entities
{
    public class User : BaseEntity
    {
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Region { get; set; }
        public string? State { get; set; }
        public string? IdentityNumber { get; set; }
        public string ImgUrl { get; set; } = string.Empty;
        public Roles Role { get; set; } = Roles.User;
    }
}
