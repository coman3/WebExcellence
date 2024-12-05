using System.Text.Json.Serialization;

namespace WebExcellence.Domain.Models
{
    public class BookItem
    {
        public string? Name { get; set; }
        public BookType Type { get; set; }
        public BookAuthor? Author { get; set; }
    }
}
