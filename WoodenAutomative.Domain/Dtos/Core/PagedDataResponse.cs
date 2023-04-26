namespace WoodenAutomative.Domain.Dtos.Core
{
    public class PagedDataResponse
    {
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
    }
}
