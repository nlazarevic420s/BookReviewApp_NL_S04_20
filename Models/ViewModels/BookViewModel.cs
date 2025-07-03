using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookReviewApp_NL_S04_20.Models.ViewModels
{
    public class BookViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(512)]
        public string Title { get; set; }

        [Range(1000, 2100)]
        public int PublicationYear { get; set; }

        [Required(ErrorMessage = "Publisher must be selected")]
        public int PublisherId { get; set; }

        public string? PublisherName { get; set; }

        public IEnumerable<SelectListItem> Publishers { get; set; } = Enumerable.Empty<SelectListItem>();

        public List<int> SelectedAuthorIds { get; set; } = new List<int>();

        public IEnumerable<SelectListItem> Authors { get; set; } = Enumerable.Empty<SelectListItem>();

        public List<int> SelectedCategoryIds { get; set; } = new List<int>();

        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();

        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }
}
