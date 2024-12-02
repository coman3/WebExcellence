using System.Text.Json.Serialization;
using WebExcellence.Application.Models;

namespace WebExcellence
{
    [JsonSerializable(typeof(BookCategory))]
    [JsonSerializable(typeof(List<BookCategory>))]
    public partial class JsonContext : JsonSerializerContext
    {
    }
}
