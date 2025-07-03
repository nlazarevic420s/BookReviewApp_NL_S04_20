using System.ComponentModel.DataAnnotations;

namespace BookReviewApp_NL_S04_20.Models.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(128)]
        public string Name { get; set; }
    }
}
