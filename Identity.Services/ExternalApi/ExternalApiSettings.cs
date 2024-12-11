namespace Identity.Services.ExternalApi
{
    public class ExternalApiSettings
    {
        public List<ApiRequestConfiguration> ApiRequest { get; set; }
        public ApiRequestConfiguration GetApiRequestByName(string name)
        {
            return ApiRequest.FirstOrDefault(x => x.Name == name)
                   ?? throw new InvalidOperationException($"API configuration '{name}' not found.");
        }
    }
    public class ApiRequestConfiguration
    {
        public string Name { get; set; }
        public KernelServer Kernel { get; set; }
        public IISServer IIS { get; set; }
        public EndPoints EndPoints { get; set; }
        public string GetFullEndpointUrl(string endpointName)
        {
            if (!EndPoints.Name.Contains(endpointName))
            {
                throw new InvalidOperationException($"Endpoint '{endpointName}' not found in configuration.");
            }

            return $"{EndPoints.Prefix}{endpointName}";
        }
    }
    public class KernelServer
    {
        public string URLHttps { get; set; }
        public string URLHttp { get; set; }
    }
    public class IISServer
    {
        public string applicationUrl { get; set; }
        public string sslUrl { get; set; }
    }
    public class EndPoints
    {
        public string Prefix { get; set; }
        public List<string> Name { get; set; }
    }
}
