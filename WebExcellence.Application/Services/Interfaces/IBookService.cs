using WebExcellence.Application.Models;
using WebExcellence.External.Api.BooksClient;

namespace WebExcellence.Application.Services.Interfaces
{
    public interface IBookService
    {
        public Task<List<BookCategory>> GetBooksCategorizedByAgeAsync(BookType? typeFilter);
    }
}
