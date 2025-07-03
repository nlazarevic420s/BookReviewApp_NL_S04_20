namespace BookReviewApp_NL_S04_20.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Publisher
    {
        public int Id { get; set; }

        [Required, StringLength(256)]
        public string Name { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
