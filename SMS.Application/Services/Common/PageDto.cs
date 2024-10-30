namespace SMS.Application.Services.Common
{
    public class PageDto
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public PageDto()
        {
            PageNumber = 1;
            PageSize = 10;
        }
    }
}
