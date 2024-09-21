namespace SMS.Common.Responses
{
    public class Response<T> : Response
    {
        public T? Result { get; set; }
    }

    public class Response
    {
        public bool Successful { get; set; }
        public string? Message { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
