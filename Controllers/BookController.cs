using BookReviewApp_NL_S04_20.DTOs;
using BookReviewApp_NL_S04_20.Models;
using BookReviewApp_NL_S04_20.Models.ViewModels;
using BookReviewApp_NL_S04_20.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookReviewApp_NL_S04_20.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly UserManager<AppUser> _userManager;
        public BookController(IBookService bookService, UserManager<AppUser> userManager)
        {
            _bookService = bookService;
            _userManager = userManager;
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder, string searchString, int page = 1, int pageSize = 5)
        {
            var options = new BookQueryOptions
            {
                SearchString = searchString,
                SortOrder = sortOrder,
                PageNumber = page,
                PageSize = 5
            };

            var (books, totalCount) = await _bookService.GetPagedBooksAsync(options);

            var viewModel = new BookIndexViewModel
            {
                Books = books,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalCount,
                SearchString = searchString,
                SortOrder = sortOrder
            };

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var viewModel = new BookViewModel
            {
                Publishers = await _bookService.GetPublishersSelectListAsync(),
                Authors = await _bookService.GetAuthorsSelectListAsync(),
                Categories = await _bookService.GetCategoriesSelectListAsync(),
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Publishers = await _bookService.GetPublishersSelectListAsync();
                model.Authors = await _bookService.GetAuthorsSelectListAsync();
                model.Categories = await _bookService.GetCategoriesSelectListAsync();
                return View(model);
            }

            await _bookService.AddBookAsync(model);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _bookService.GetBookForEditAsync(id);
            if (model == null)
            {
                return NotFound();
            }
               
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                model.Publishers = await _bookService.GetPublishersSelectListAsync();
                model.Authors = await _bookService.GetAuthorsSelectListAsync(model.SelectedAuthorIds);
                model.Categories = await _bookService.GetCategoriesSelectListAsync(model.SelectedCategoryIds);
                return View(model);
            }

            var updated = await _bookService.UpdateBookAsync(id, model);
            if (!updated)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var viewModel = await _bookService.GetBookForDeleteAsync(id);
            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _bookService.DeleteBookAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var currentUserId = User.Identity.IsAuthenticated
                ? _userManager.GetUserId(User)
                : null;

            var viewModel = await _bookService.GetBookDetailsAsync(id, currentUserId);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }
    }
}
