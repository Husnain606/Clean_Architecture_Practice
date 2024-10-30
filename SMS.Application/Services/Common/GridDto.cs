namespace SMS.Application.Services.Common
{
    public class GridDto<T> where T : class
    {
        public List<T> Data { get; set; } = null!;
        public int TotalRecords { get; set; }
    }
}
