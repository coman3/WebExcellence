using System.Text.Json.Serialization;
using WebExcellence.Domain.Models;

namespace WebExcellence
{
    [JsonSerializable(typeof(BookCategory))]
    [JsonSerializable(typeof(List<BookCategory>))]
    [JsonSerializable(typeof(BookAuthor))]
    [JsonSerializable(typeof(BookItem))]
    [JsonSerializable(typeof(AgeCategory))]
    public partial class JsonContext : JsonSerializerContext
    {
    }
}
