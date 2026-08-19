namespace UrlShortener.Application.Common
{
    /// <summary>
    /// A simple result wrapper so services can report expected failures
    /// (e.g. "username taken", "invalid credentials") without throwing exceptions
    /// for control flow.
    /// </summary>
    public class ServiceResult<T>
    {
        public bool Success { get; private set; }
        public string? Error { get; private set; }
        public T? Data { get; private set; }

        public static ServiceResult<T> Ok(T data) => new ServiceResult<T>
        {
            Success = true,
            Data = data
        };

        public static ServiceResult<T> Fail(string error) => new ServiceResult<T>
        {
            Success = false,
            Error = error
        };
    }
}
