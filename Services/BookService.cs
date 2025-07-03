using Microsoft.EntityFrameworkCore;
using BookReviewApp_NL_S04_20.Data;
using BookReviewApp_NL_S04_20.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookReviewApp_NL_S04_20.Models;

namespace BookReviewApp_NL_S04_20.Services
{
    public interface IBookService
    {
        Task<List<BookViewModel>> GetAllBooksWithRatingsAsync(); 
        Task<IEnumerable<SelectListItem>> GetPublishersSelectListAsync(); 
        Task<IEnumerable<SelectListItem>> GetAuthorsSelectListAsync(List<int> selectedIds = null);
        Task<IEnumerable<SelectListItem>> GetCategoriesSelectListAsync(List<int> selectedIds = null);
        Task AddBookAsync(BookViewModel model);
        Task<BookViewModel?> GetBookForEditAsync(int id);
        Task<BookViewModel?> GetBookForDeleteAsync(int id);

        Task<bool> UpdateBookAsync(int id, BookViewModel model);
        Task<bool> DeleteBookAsync(int id);

        Task<BookDetailsViewModel?> GetBookDetailsAsync(int bookId, string? currentUserId);
    }

    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;

        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<BookViewModel>> GetAllBooksWithRatingsAsync()
        {
            return await _context.Books
                .Include(b => b.Publisher)
                .Include(b => b.Reviews)
                .Select(b => new BookViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    PublicationYear = b.PublicationYear,
                    PublisherId = b.PublisherId,
                    PublisherName = b.Publisher.Name,
                    AverageRating = b.Reviews.Any() ? b.Reviews.Average(r => r.Rating) : 0,
                    ReviewCount = b.Reviews.Count()
                })
                .OrderBy(b => b.Title)
                .ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetPublishersSelectListAsync()
        {
            return await _context.Publishers
                .OrderBy(p => p.Name)
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetAuthorsSelectListAsync(List<int> selectedIds = null)
        {
            selectedIds ??= new List<int>();

            return await _context.Authors
                .OrderBy(a => a.Name)
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Name,
                    Selected = selectedIds.Contains(a.Id)
                }).ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetCategoriesSelectListAsync(List<int> selectedIds = null)
        {
            selectedIds ??= new List<int>();

            return await _context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = selectedIds.Contains(c.Id)
                }).ToListAsync();
        }

        public async Task AddBookAsync(BookViewModel model)
        {
            var book = new Book
            {
                Title = model.Title,
                PublicationYear = model.PublicationYear,
                PublisherId = model.PublisherId,
            };

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            if (model.SelectedAuthorIds != null)
            {
                foreach (var authorId in model.SelectedAuthorIds)
                {
                    _context.AuthorBooks.Add(new AuthorBook
                    {
                        BookId = book.Id,
                        AuthorId = authorId
                    });
                }
            }

            if (model.SelectedCategoryIds != null)
            {
                foreach (var categoryId in model.SelectedCategoryIds)
                {
                    _context.BookCategories.Add(new BookCategory
                    {
                        BookId = book.Id,
                        CategoryId = categoryId
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<BookViewModel?> GetBookForEditAsync(int id)
        {
            var book = await _context.Books
                .Include(b => b.AuthorBooks)
                .Include(b => b.BookCategories)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            { 
                return null;
            }

            var selectedAuthorIds = book.AuthorBooks.Select(ab => ab.AuthorId).ToList();
            var selectedCategoryIds = book.BookCategories.Select(bc => bc.CategoryId).ToList();

            return new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                PublicationYear = book.PublicationYear,
                PublisherId = book.PublisherId,
                SelectedAuthorIds = selectedAuthorIds,
                SelectedCategoryIds = selectedCategoryIds,
                Publishers = await GetPublishersSelectListAsync(),
                Authors = await GetAuthorsSelectListAsync(selectedAuthorIds),
                Categories = await GetCategoriesSelectListAsync(selectedCategoryIds)
            };
        }

        public async Task<BookViewModel?> GetBookForDeleteAsync(int id)
        {
            var book = await _context.Books
                .Include(b => b.Publisher)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return null;
            }

            return new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                PublicationYear = book.PublicationYear,
                PublisherId = book.PublisherId,
                PublisherName = book.Publisher?.Name
            };
        }

        public async Task<bool> UpdateBookAsync(int id, BookViewModel model)
        {
            var book = await _context.Books
                .Include(b => b.AuthorBooks)
                .Include(b => b.BookCategories)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return false;
            }

            book.Title = model.Title;
            book.PublicationYear = model.PublicationYear;
            book.PublisherId = model.PublisherId;

            book.AuthorBooks.Clear();
            if (model.SelectedAuthorIds != null)
            {
                foreach (var authorId in model.SelectedAuthorIds)
                {
                    book.AuthorBooks.Add(new AuthorBook { BookId = id, AuthorId = authorId });
                }
            }

            book.BookCategories.Clear();
            if (model.SelectedCategoryIds != null)
            {
                foreach (var categoryId in model.SelectedCategoryIds)
                {
                    book.BookCategories.Add(new BookCategory { BookId = id, CategoryId = categoryId });
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await _context.Books
                .Include(b => b.AuthorBooks)
                .Include(b => b.BookCategories)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return false;
            }
                
            _context.AuthorBooks.RemoveRange(book.AuthorBooks);
            _context.BookCategories.RemoveRange(book.BookCategories);
            _context.Books.Remove(book);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<BookDetailsViewModel?> GetBookDetailsAsync(int bookId, string? currentUserId)
        {
            var book = await _context.Books
                .Include(b => b.Publisher)
                .Include(b => b.AuthorBooks).ThenInclude(ab => ab.Author)
                .Include(b => b.BookCategories).ThenInclude(bc => bc.Category)
                .Include(b => b.Reviews).ThenInclude(r => r.User)
                .FirstOrDefaultAsync(b => b.Id == bookId);

            if (book == null)
            {
                return null;
            }

            var reviews = book.Reviews
                .OrderByDescending(r => r.PublishedDate)
                .Select(r => new ReviewViewModel
                {
                    Id = r.Id,
                    Text = r.Text,
                    Rating = r.Rating,
                    PublishedDate = r.PublishedDate,
                    Username = r.User.UserName,
                    UserId = r.UserId
                }).ToList();

            return new BookDetailsViewModel
            {
                Id = book.Id,
                Title = book.Title,
                PublicationYear = book.PublicationYear,
                PublisherName = book.Publisher?.Name ?? "Unknown",
                Authors = book.AuthorBooks.Select(ab => ab.Author.Name).ToList(),
                Categories = book.BookCategories.Select(bc => bc.Category.Name).ToList(),
                AverageRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0,
                ReviewCount = reviews.Count,
                Reviews = reviews,
                CurrentUserId = currentUserId
            };
        }
    }
}
