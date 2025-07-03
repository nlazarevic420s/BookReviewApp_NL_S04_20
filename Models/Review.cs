namespace BookReviewApp_NL_S04_20.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Review
    {
        public int Id { get; set; }

        [Required, StringLength(2048)]
        public string Text { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; } = DateTime.Now;

        public string UserId { get; set; }
        public AppUser? User { get; set; }

        public int BookId { get; set; }
        public Book? Book { get; set; }
    }
}
