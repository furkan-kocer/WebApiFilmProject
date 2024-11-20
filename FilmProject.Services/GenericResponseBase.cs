using Serilog;

namespace FilmProject.Services
{
    public class GenericResponseBase<T>
    {
        public bool result { get; set; }
        public T? data { get; set; }
        public string? errorDetail { get; set; }
        public static GenericResponseBase<T> Error(string error)
        {
            Log.Logger.Information("Something went wrong.{error}", error);
            return new GenericResponseBase<T>
            {
                result = false,
                errorDetail = error
            };
        }
        public static GenericResponseBase<T> NotFound(string message)
        {
            Log.Logger.Information("NotFound function executed within message: {message}", message);
            return new GenericResponseBase<T>
            {
                result = false,
                errorDetail = message
            };
        }
        public static GenericResponseBase<T> Success()
        {
            Log.Logger.Information("The Success method executed without data.");
            return new GenericResponseBase<T>
            {
                result = true,
            };
        }
        public static GenericResponseBase<T> Success(T data)
        {
            Log.Logger.Information("The Success method executed with data.");
            return new GenericResponseBase<T>
            {
                result = true,
                data = data,
            };
        }
    }
}
