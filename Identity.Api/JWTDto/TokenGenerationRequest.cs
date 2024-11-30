using Identity.Api.Modal;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Identity.Api.JWTDto
{
    public class TokenGenerationRequest
    {
        public Guid userID { get; set; }
        [Required]
        [NotNull]
        [MaxLength(100)]
        public string username { get; set; }
        [Required]
        [NotNull]
        [MaxLength(100)]
        public string email { get; set; }
        public Roles Role { get; set; }

        //public CustomClaims customclaims { get; set; }
    }
}
