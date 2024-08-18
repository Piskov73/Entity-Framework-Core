namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using System.Globalization;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var context = new BookShopContext();
            DbInitializer.ResetDatabase(context);
           
        }
        // 02. Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            if (!Enum.TryParse(command, true, out AgeRestriction restriction))
            {
                return "";
            }
            var booksAgeRestriction = context.Books.Where(b => b.AgeRestriction == restriction)
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToList();
            return string.Join(Environment.NewLine, booksAgeRestriction);
        }

        //03. Golden Books
        public static string GetGoldenBooks(BookShopContext context)
        {
            var goldenBooks = context.Books.ToArray()
                .Where(b => b.Copies < 5000 && b.EditionType.ToString() == "Gold")
                .Select(b => new
                {
                    b.BookId,
                    b.Title
                }).OrderBy(b => b.BookId).ToArray();
            return string.Join(Environment.NewLine, goldenBooks.Select(b => b.Title));
        }

        //04. Books by Price

        public static string GetBooksByPrice(BookShopContext context)
        {
            var BooksByPrice = context.Books
                .Where(b => b.Price > 40)
                .OrderByDescending(b => b.Price)
                .Select(b => $"{b.Title} - ${b.Price:f2}")
                .ToArray();

            return string.Join(Environment.NewLine, BooksByPrice);
        }

        //05. Not Released In

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var BooksNotReleasedIn = context.Books
                .Where(b => b.ReleaseDate.Value.Year != year)
                .Select(b => new
                {
                    b.BookId,
                    b.Title
                })
                .OrderBy(b => b.BookId)
                .ToArray();

            return string.Join(Environment.NewLine, BooksNotReleasedIn.Select(b => b.Title));
        }

        //06. Book Titles by Category

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] booxsCategiry = input.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries).ToArray();

            var BooxsByCategory = context.BooksCategories.Where(bc => booxsCategiry.Contains(bc.Category.Name.ToLower()))
                .Select(bc => bc.Book.Title)
                .OrderBy(t => t)
                .ToArray();


            return string.Join(Environment.NewLine, BooxsByCategory);
        }

        //07. Released Before Date

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {

            var parsedDate = DateTime.ParseExact(date, "dd-MM-yyyy", null);

            var BooksReleased = context.Books.Where(b => b.ReleaseDate < parsedDate)
                .OrderByDescending(b => b.ReleaseDate)
            .Select(b => $"{b.Title} - {b.EditionType.ToString()} - ${b.Price:f2}")
            .ToArray();
            return string.Join(Environment.NewLine, BooksReleased);
        }

        //08. Author Search

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var AuthorsName = context.Authors.Where(a => a.FirstName.EndsWith(input))
                .Select(a => $"{a.FirstName} {a.LastName}")
                .ToArray()
                .OrderBy(a => a);

            return string.Join(Environment.NewLine, AuthorsName);
        }

        //09. Book Search

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var BookTitles = context.Books.Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(a => a.Title)
                .Select(a => a.Title)
                .ToArray();
            return string.Join(Environment.NewLine, BookTitles);
        }

        //10. Book Search by Author

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var BoksByAuthor = context.Books.Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    b.Title,
                    AuthorFulName = $"({b.Author.FirstName} {b.Author.LastName})"
                }).ToArray();


            StringBuilder sb = new StringBuilder();

            foreach (var boks in BoksByAuthor)
            {
                sb.AppendLine($"{boks.Title} {boks.AuthorFulName}");
            }

            return sb.ToString().TrimEnd();
        }

        //11. Count Books
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var Count = context.Books.Where(b => b.Title.Length > lengthCheck).ToArray().Count();
            return Count;
        }

        //12. Total Book Copies

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var CopiesByAuthor = context.Authors
                .Select(a => new
                {
                    FulName = $"{a.FirstName} {a.LastName}",

                    Copiues = a.Books.Sum(b => b.Copies)

                })
                .OrderByDescending(a => a.Copiues)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var item in CopiesByAuthor)
            {
                sb.AppendLine($"{item.FulName} - {item.Copiues}");
            }
            return sb.ToString().TrimEnd();
        }

        //13. Profit by Category

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var TotalProfitCategory = context.Categories
                .Select(c => new
                {
                    c.Name,
                    TotalPrice = c.CategoryBooks.Sum(cb => cb.Book.Copies * cb.Book.Price)
                })
                .OrderByDescending(bc => bc.TotalPrice)
                .ThenBy(bc => bc.Name)
                .ToArray();


            return string.Join(Environment.NewLine, TotalProfitCategory.Select(t => $"{t.Name} ${t.TotalPrice:f2}"));
        }

        //14. Most Recent Books

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var BoxsCategories = context.Categories.OrderBy(c => c.Name)
                .Select(c => new
                {
                    c.Name,
                    Boxs = c.CategoryBooks.OrderByDescending(c => c.Book.ReleaseDate)
                    .Select(cb => new
                    {
                        cb.Book.Title,
                        cb.Book.ReleaseDate.Value.Year
                    }).Take(3)

                }).OrderBy(c => c.Name).ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var bc in BoxsCategories)
            {
                sb.AppendLine($"--{bc.Name}");

                foreach (var b in bc.Boxs)
                {
                    sb.AppendLine($"{b.Title} ({b.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        //15. Increase Prices

        public static void IncreasePrices(BookShopContext context)
        {
            var Boxs = context.Books.Where(b => b.ReleaseDate.Value.Year < 2010).ToArray();

            foreach (var b in Boxs)
            {
                b.Price += 5;
            }

            context.SaveChanges();  
        }

        //16. Remove Books

        public static int RemoveBooks(BookShopContext context)
        {
            var remuveBooks = context.Books.Where(b => b.Copies < 4200).ToList();

            foreach (var rb in remuveBooks)
            {
                context.Books.Remove(rb);
            }

            context.SaveChanges();

            return remuveBooks.Count();
        }
    }
}


