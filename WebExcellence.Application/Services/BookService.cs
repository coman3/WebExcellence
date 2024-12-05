using Microsoft.Extensions.Options;
using WebExcellence.Application.Configuration;
using WebExcellence.Application.Services.Interfaces;
using WebExcellence.Domain.Models;
using WebExcellence.External.Api.BooksClient;

namespace WebExcellence.Application.Services
{
    public class BookService(ISwaggerBooksClient booksApiClient, IOptions<AgeCategoryOptions> ageCategoryOptions) : IBookService
    {
        private List<AgeCategory> AgeCategories => ageCategoryOptions.Value.Categories;
        private string? GetAgeCategoryName(int age)
        {
            var category = AgeCategories.FirstOrDefault(c => age >= c.MinAge && age <= c.MaxAge);
            return category?.Name;

        }

        public async Task<List<BookCategory>> GetBooksCategorizedByAgeAsync(Domain.Models.BookType? typeFilter)
        {
            ICollection<BookOwner>? bookOwners;

            try
            {
                bookOwners = await booksApiClient.BookownersAsync();
            }
            catch (Exception ex)
            {
                // TODO: Log exception to seq
                throw new Exception("Failed to retrieve books from the external API.", ex);
            }

            var bookCategories = new List<BookCategory>();

            if (bookOwners == null)
            {
                return bookCategories;
            }

            var categorizedBooks = new Dictionary<string, List<BookItem>>();


            foreach (var owner in bookOwners)
            {
                var ageCategoryName = GetAgeCategoryName(owner.Age);

                if (string.IsNullOrEmpty(ageCategoryName))
                {
                    throw new InvalidOperationException("Age category not found for owner.");
                }


                if (!categorizedBooks.TryGetValue(ageCategoryName, out List<BookItem>? value))
                {
                    value = [];
                    categorizedBooks[ageCategoryName] = value;
                }



                if (owner.Books != null)
                {
                    foreach (var book in owner.Books)
                    {
                        var bookType = Enum.Parse<Domain.Models.BookType>(book.Type.ToString(), true); // Match based on name, not integer value;
                        if (typeFilter == null || bookType == typeFilter)
                        {
                            value.Add(new BookItem
                            {
                                Name = book.Name,
                                Type = bookType,
                                Author = new BookAuthor { Name = owner.Name, Age = owner.Age }
                            });
                        }
                    }
                }

            }

            foreach (var kvp in categorizedBooks.Where(x => x.Value.Count > 0))
            {
                kvp.Value.Sort((b1, b2) => string.Compare(b1.Name, b2.Name, StringComparison.Ordinal));
                bookCategories.Add(new BookCategory { Name = kvp.Key, Books = kvp.Value });
            }

            bookCategories.Sort((b1, b2) => string.Compare(b1.Name, b2.Name, StringComparison.Ordinal));

            return bookCategories;
        }
    }
}
