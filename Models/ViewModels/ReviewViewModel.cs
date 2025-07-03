namespace BookReviewApp_NL_S04_20.Models.ViewModels
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Username { get; set; }
        public string UserId { get; set; }
    }
}
