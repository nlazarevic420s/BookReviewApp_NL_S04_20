using System.ComponentModel.DataAnnotations;

namespace BookReviewApp_NL_S04_20.DTOs
{
    public class ReviewCreateDto
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        [StringLength(2048)]
        public string Text { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }
    }
}
