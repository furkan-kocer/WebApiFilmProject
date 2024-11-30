using Identity.Api.Modal;

namespace Identity.Api.JWTDto
{
    public class TokenGenerationRequest
    {
        public Guid userID { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public Roles Role { get; set; }

        //public CustomClaims customclaims { get; set; }
    }
}
