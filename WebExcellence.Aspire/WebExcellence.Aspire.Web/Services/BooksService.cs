using WebExcellence.Domain.Models;

namespace WebExcellence.Aspire.Web.Services
{
    public class BookService
    {
        private readonly HttpClient _httpClient;

        public BookService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<BookCategory>> GetBooksAsync(BookType? bookType = null)
        {
            // Hacky, manual way of doing this for time sake - you can see how i would do this for the external books api referenced in other projects.
            var query = bookType.HasValue ? $"?book-type={(int)bookType.Value}" : "";
            return await _httpClient.GetFromJsonAsync<List<BookCategory>>($"/books{query}") ?? [];
        }
    }
}
