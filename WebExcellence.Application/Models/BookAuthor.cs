using System.Text.Json.Serialization;

namespace WebExcellence.Application.Models
{
    [JsonSerializable(typeof(BookAuthor))]
    public class BookAuthor
    {
        public string? Name { get; set; }
        public int Age { get; set; }
    }
}
