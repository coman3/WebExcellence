using System.Text.Json.Serialization;

namespace WebExcellence.Domain.Models
{
    public class BookCategory
    {
        public string? Name { get; set; }
        public List<BookItem>? Books { get; set; }
    }
}
