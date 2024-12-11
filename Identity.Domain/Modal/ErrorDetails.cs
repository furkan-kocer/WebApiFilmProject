using System.Text.Json;

namespace Identity.Domain.Modal
{
    public class ErrorDetails
    {
        public int statusCode { get; set; }
        public string? message { get; set; }
        public override string ToString() => JsonSerializer.Serialize(this);
    }
}
