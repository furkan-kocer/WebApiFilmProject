using Serilog;
using System.Text.Json;
using System.Text;

namespace Identity.Services.ExternalApi
{
    public class ExternalApiService : IExternalApiService
    {
        private readonly HttpClient _httpClient;

        public ExternalApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetExternalDataAsync(string endpoint)
        {
            Log.Logger.Information("{endpoint} call requested with the name of {methodname} method.", endpoint, nameof(GetExternalDataAsync));

            var response = await _httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            // Handle errors as needed
            throw new HttpRequestException("Error calling external API");
        }

        public async Task<T> PostToExternalApiAsync<T>(string endpoint, object content)
        {
            Log.Logger.Information("{endpoint} call requested with the name of {methodname} method.", endpoint, nameof(PostToExternalApiAsync));

            var jsonContent = JsonSerializer.Serialize(content);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, httpContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true 
                };

                return JsonSerializer.Deserialize<T>(responseContent, options);
            }

            throw new HttpRequestException($"Error calling external API. Status Code: {response.StatusCode}, Response: {responseContent}");
        }
    }
}
