using FilmProject.DataAccess.FilterErrors;
using Microsoft.AspNetCore.Mvc;

namespace FilmProject.Api.FilterError
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
