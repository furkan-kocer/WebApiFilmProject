namespace Identity.Api.Modal
{
    public class CustomClaims
    {
        public Roles Role { get; set; }
        public bool CanEditMovies { get; set; }
        public bool CanViewReports { get; set; }
        public string SubscriptionLevel { get; set; } // e.g., Basic, Premium, VIP
    }
}
