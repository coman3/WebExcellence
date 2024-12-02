using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebExcellence.Application.Models;
using WebExcellence.External.Api.BooksClient;

namespace WebExcellence.Application.Services.Interfaces
{
    public interface IBookService
    {
        public Task<List<BookCategory>> GetBooksCategorizedByAgeAsync(BookType? typeFilter);
    }
}
