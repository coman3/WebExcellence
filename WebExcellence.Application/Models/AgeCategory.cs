using System.Text.Json.Serialization;

namespace WebExcellence.Application.Models
{
    [JsonSerializable(typeof(AgeCategory))]
    public class AgeCategory
    {
        public string Name { get; set; } = "";
        public int MinAge { get; set; }
        public int MaxAge { get; set; } // Inclusive upper bound
    }
}
