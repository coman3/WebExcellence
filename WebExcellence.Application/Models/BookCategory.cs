using System.Text.Json.Serialization;

namespace WebExcellence.Application.Models
{
    [JsonSerializable(typeof(BookCategory))]
    [JsonSerializable(typeof(List<BookCategory>))]
    public class BookCategory
    {
        public string? Name { get; set; }
        public List<BookItem>? Books { get; set; }
    }
}
