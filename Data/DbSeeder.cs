using BookReviewApp_NL_S04_20.Constants;
using BookReviewApp_NL_S04_20.Models;
using Microsoft.AspNetCore.Identity;

namespace BookReviewApp_NL_S04_20.Data
{
    public class DbSeeder
    {
        private readonly ApplicationDbContext _context;

        public DbSeeder(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (_context.Books.Any())
            {
                return;
            }

            // --- 1. Autori ---
            var authors = new List<Author>
            {
                new Author { Name = "George Orwell" }, // ID 1
                new Author { Name = "Aldous Huxley" }, // ID 2
                new Author { Name = "J.K. Rowling" },  // ID 3
                new Author { Name = "J.R.R. Tolkien" },// ID 4
                new Author { Name = "Stephen King" },  // ID 5
                new Author { Name = "Dan Brown" },     // ID 6
            };
            await _context.Authors.AddRangeAsync(authors);
            await _context.SaveChangesAsync();

            // --- 2. Izdavači ---
            var publishers = new List<Publisher>
            {
                new Publisher { Name = "Penguin Books" },// ID 1
                new Publisher { Name = "HarperCollins" },// ID 2
                new Publisher { Name = "Bloomsbury" },   // ID 3
            };
            await _context.Publishers.AddRangeAsync(publishers);
            await _context.SaveChangesAsync();

            // --- 3. Kategorije ---
            var categories = new List<Category>
            {
                new Category { Name = "Science Fiction" },// ID 1
                new Category { Name = "Fantasy" },        // ID 2
                new Category { Name = "Thriller" },       // ID 3
                new Category { Name = "Classic" },        // ID 4
                new Category { Name = "Horror" },         // ID 5
            };
            await _context.Categories.AddRangeAsync(categories);
            await _context.SaveChangesAsync();

            // --- 4. Knjige ---
            var books = new List<Book>
            {
                new Book { Title = "1984", PublicationYear = 1949, PublisherId = 1 },                  // ID 1
                new Book { Title = "Brave New World", PublicationYear = 1932, PublisherId = 1 },       // ID 2
                new Book { Title = "Harry Potter", PublicationYear = 1997, PublisherId = 3 },          // ID 3
                new Book { Title = "The Hobbit", PublicationYear = 1937, PublisherId = 2 },            // ID 4
                new Book { Title = "It", PublicationYear = 1986, PublisherId = 2 },                    // ID 5
                new Book { Title = "The Da Vinci Code", PublicationYear = 2003, PublisherId = 1 },     // ID 6
                new Book { Title = "The Shining", PublicationYear = 1977, PublisherId = 2 },           // ID 7
                new Book { Title = "Angels & Demons", PublicationYear = 2000, PublisherId = 1 },       // ID 8
                new Book { Title = "Fantastic Beasts", PublicationYear = 2016, PublisherId = 3 },      // ID 9
                new Book { Title = "The Lord of the Rings", PublicationYear = 1954, PublisherId = 2 }, // ID 10
            };
            await _context.Books.AddRangeAsync(books);
            await _context.SaveChangesAsync();

            // --- 5. Relacija: BookCategory ---
            var bookCategories = new List<BookCategory>
            {
                new BookCategory { BookId = 1, CategoryId = 4 },  // 1984 -> Classic
                new BookCategory { BookId = 2, CategoryId = 1 },  // Brave New World -> Sci-Fi
                new BookCategory { BookId = 3, CategoryId = 2 },  // HP -> Fantasy
                new BookCategory { BookId = 4, CategoryId = 2 },  // Hobbit -> Fantasy
                new BookCategory { BookId = 5, CategoryId = 5 },  // It -> Horror
                new BookCategory { BookId = 6, CategoryId = 3 },  // Da Vinci -> Thriller
                new BookCategory { BookId = 7, CategoryId = 5 },  // Shining -> Horror
                new BookCategory { BookId = 8, CategoryId = 3 },  // Angels -> Thriller
                new BookCategory { BookId = 9, CategoryId = 2 },  // Fantastic Beasts -> Fantasy
                new BookCategory { BookId = 10, CategoryId = 2 }, // LOTR -> Fantasy
                new BookCategory { BookId = 10, CategoryId = 4 }, // LOTR -> Classic
            };
            await _context.BookCategories.AddRangeAsync(bookCategories);
            await _context.SaveChangesAsync();

            // --- 6. Relacija: AuthorBook ---
            var authorBooks = new List<AuthorBook>
            {
                new AuthorBook { AuthorId = 1, BookId = 1 },  // Orwell -> 1984
                new AuthorBook { AuthorId = 2, BookId = 2 },  // Huxley -> Brave New World
                new AuthorBook { AuthorId = 3, BookId = 3 },  // Rowling -> HP
                new AuthorBook { AuthorId = 4, BookId = 4 },  // Tolkien -> Hobbit
                new AuthorBook { AuthorId = 5, BookId = 5 },  // King -> It
                new AuthorBook { AuthorId = 6, BookId = 6 },  // Brown -> Da Vinci
                new AuthorBook { AuthorId = 5, BookId = 7 },  // King -> Shining
                new AuthorBook { AuthorId = 6, BookId = 8 },  // Brown -> Angels
                new AuthorBook { AuthorId = 3, BookId = 9 },  // Rowling -> Fantastic Beasts
                new AuthorBook { AuthorId = 4, BookId = 10 }, // Tolkien -> LOTR
                new AuthorBook { AuthorId = 3, BookId = 10 }, // Rowling + Tolkien -> LOTR (2 autora)
            };
            await _context.AuthorBooks.AddRangeAsync(authorBooks);
            await _context.SaveChangesAsync();
        }

        public static async Task SeedRolesAsync(IServiceProvider service)
        {
            var userManager = service.GetService<UserManager<AppUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();

            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));

            var user = new AppUser
            {
                UserName = "admin@bookapp.rs",
                Email = "admin@bookapp.rs",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            var userInDb = await userManager.FindByEmailAsync(user.Email);
            if (userInDb == null)
            {
                var result = await userManager.CreateAsync(user, "Admin123$");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                }
            }
        }
    }
}
