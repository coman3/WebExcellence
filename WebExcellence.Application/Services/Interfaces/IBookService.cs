using WebExcellence.Domain.Models;
namespace WebExcellence.Application.Services.Interfaces
{
    public interface IBookService
    {
        public Task<List<BookCategory>> GetBooksCategorizedByAgeAsync(BookType? typeFilter);
    }
}
