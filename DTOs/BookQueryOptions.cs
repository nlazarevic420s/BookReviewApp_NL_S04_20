namespace BookReviewApp_NL_S04_20.DTOs
{
    public class BookQueryOptions
    {
        public string? SearchString { get; set; }
        public string? SortOrder { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
