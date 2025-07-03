namespace BookReviewApp_NL_S04_20.Models
{
    using Microsoft.AspNetCore.Identity;

    public class AppUser : IdentityUser
    {
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
