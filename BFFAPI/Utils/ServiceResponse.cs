namespace BFFAPI.Utils
{
    public class ServiceResponse
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ServiceResponse<T> : ServiceResponse
    {
        public T Data { get; set; }
    }
}
