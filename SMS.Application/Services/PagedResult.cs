namespace SMS.Application.Services
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalRecords { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((double)TotalRecords / PageSize);
            }
        }
    }

}
