namespace FilmProject.Contracts.FilterErrors
{
    public class FilterErrorDetails
    {
        public string title { get; set; }
        public string type { get; set; }    
        public string detail { get; set; }
        public int? statusCode { get; set; }
        public FilterErrorModelResponse errorModelResponse { get; set; }
    }
}
