using System.Text.Json.Serialization;

namespace WebExcellence.Domain.Models
{
    public class AgeCategory
    {
        public string Name { get; set; } = "";
        public int MinAge { get; set; }
        public int MaxAge { get; set; } // Inclusive upper bound
    }
}
