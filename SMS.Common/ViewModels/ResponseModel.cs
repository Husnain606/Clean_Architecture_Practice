using System.Net;

namespace SMS.Common.ViewModels
{
    public class ResponseModel
    {
        public bool IsSuccess {  get;  set; }
        public HttpStatusCode StatusCode { get; set; }
        public dynamic data { get; set; }
        public string Messsage  { get; set; } = string.Empty;
    }
}
