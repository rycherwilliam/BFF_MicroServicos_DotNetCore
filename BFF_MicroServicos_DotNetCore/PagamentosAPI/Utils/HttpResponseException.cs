namespace PagamentosAPI.Utils{ 
    public class HttpResponseException : Exception
    {
        public int StatusCode { get; }

        public HttpResponseException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}