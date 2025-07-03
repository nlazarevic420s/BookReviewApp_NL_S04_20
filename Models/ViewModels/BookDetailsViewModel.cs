namespace BookReviewApp_NL_S04_20.Models.ViewModels
{
    public class BookDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public int PublicationYear { get; set; }

        public string PublisherName { get; set; }

        public List<string> Authors { get; set; }
        public List<string> Categories { get; set; }

        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }

        public string? CurrentUserId { get; set; }

        public List<ReviewViewModel> Reviews { get; set; }
    }
}
