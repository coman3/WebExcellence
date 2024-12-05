using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using WebExcellence.Application.Services.Interfaces;
using WebExcellence.External.Api.BooksClient;

namespace WebExcellence.Aspire.ApiService.Features.Books
{
    public static class BooksEndpoints
    {
        public static void RegisterBooksEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/books",
                (
                    [FromServices] IBookService bookService,
                    [FromQuery(Name = "book-type")]
                    BookType? bookType
                ) => bookService.GetBooksCategorizedByAgeAsync(bookType))
                .WithName("GetBooks")
                .WithOpenApi(x => new OpenApiOperation(x)
                {
                    Summary = "Get Books by age category",
                    Description = "Returns all books grouped by category.",
                    Tags = new List<OpenApiTag> { new() { Name = "Books" } }
                });
        }
    }
}
