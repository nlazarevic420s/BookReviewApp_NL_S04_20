namespace BookReviewApp_NL_S04_20.Constants
{
    public enum Roles
    {
        Admin,
        User
    }
    public static class BookSortOptions
    {
        public const string TitleDesc = "title_desc";
        public const string YearAsc = "year_asc";
        public const string YearDesc = "year_desc";
        public const string RatingAsc = "rating_asc";
        public const string RatingDesc = "rating_desc";

        public static readonly string[] All =
        {
            TitleDesc,
            YearAsc,
            YearDesc,
            RatingAsc,
            RatingDesc
        };

    }
}
