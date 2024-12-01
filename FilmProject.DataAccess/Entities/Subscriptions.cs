namespace FilmProject.DataAccess.Entities
{
    public class Subscriptions : BaseEntity
    {
        public string SubscriptionName { get; set; }
        public string SubscriptionCode { get; set; }
        public string Resolution { get; set; }
        public float MonthlyPrice { get; set; }
    }
}
