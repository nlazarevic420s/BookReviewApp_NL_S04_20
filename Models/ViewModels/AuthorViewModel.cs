using System.ComponentModel.DataAnnotations;

namespace BookReviewApp_NL_S04_20.Models.ViewModels
{
    public class AuthorViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(256)]
        public string Name { get; set; }
    }
}
