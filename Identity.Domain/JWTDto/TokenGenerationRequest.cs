using Identity.Domain.Modal;
using System.ComponentModel.DataAnnotations;
namespace Identity.Domain.JWTDto
{
    public class TokenGenerationRequest
    {
        public string userID { get; set; }
        [MaxLength(20)]
        public string? phoneNumber { get; set; }
        [MaxLength(100)]
        public string email { get; set; }

        [Required]
        [EnumDataType(typeof(Roles))]
        public Roles Role { get; set; }

        //public CustomClaims customclaims { get; set; }
    }
}
