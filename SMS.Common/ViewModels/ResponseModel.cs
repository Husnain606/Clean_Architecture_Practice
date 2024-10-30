using System.Net;

namespace SMS.Common.ViewModels
{
    public class ResponseModel<T> : ResponseModel
    {
        public T? Result { get; set; }
    }

    public class ResponseModel
    {
        public bool IsSuccess { get; set; }
        public string? Messsage { get; set; }
        public dynamic data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public string Message { get; set; }
    }
}
