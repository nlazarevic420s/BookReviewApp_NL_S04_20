namespace BookReviewApp_NL_S04_20.Models.ViewModels
{
    public class BookIndexViewModel
    {
        public List<BookViewModel> Books { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        public string SearchString { get; set; }
        public string SortOrder { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}
