namespace Identity.Services.ExternalApi
{
    public interface IExternalApiService
    {
        Task<string> GetExternalDataAsync(string endpoint);
        Task<T> PostToExternalApiAsync<T>(string endpoint, object content);
    }
}
