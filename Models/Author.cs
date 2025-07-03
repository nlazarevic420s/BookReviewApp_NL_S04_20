namespace BookReviewApp_NL_S04_20.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Author
    {
        public int Id { get; set; }

        [Required, StringLength(256)]
        public string Name { get; set; }

        public ICollection<AuthorBook> AuthorBooks { get; set; } = new List<AuthorBook>();
    }
}
