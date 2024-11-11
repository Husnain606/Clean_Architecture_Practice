using System.Net;

namespace SMS.Common.ViewModels
{
    public class ResponseModel<T> : ResponseModel
    {
        public T? Result { get; set; }
      //  public IList<T>? Result { get; set; }
    }

    public class ResponseModel
    {
        public bool Successful { get; set; }
        public string? Message { get; set; }
        public IEnumerable<string>? Errors { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
