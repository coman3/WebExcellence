using System.Text.Json.Serialization;
using WebExcellence.External.Api.BooksClient;

namespace WebExcellence.Application.Models
{
    [JsonSerializable(typeof(BookItem))]
    public class BookItem
    {
        public string? Name { get; set; }
        public BookType Type { get; set; }
        public BookAuthor? Author { get; set; }
    }
}
