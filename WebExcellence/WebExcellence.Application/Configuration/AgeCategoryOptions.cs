using WebExcellence.Application.Models;
using WebExcellence.Application.Services;

namespace WebExcellence.Application.Configuration
{
    public class AgeCategoryOptions
    {
        public List<AgeCategory> Categories { get; set; } = new List<AgeCategory>();

        public void ValidateCategories()
        {
            if (Categories == null || Categories.Count == 0)
            {
                throw new ArgumentException("Age category configuration is missing or empty.");
            }

            for (int i = 0; i < Categories.Count; i++)
            {
                for (int j = i + 1; j < Categories.Count; j++)
                {
                    if (Categories[i].MaxAge >= Categories[j].MinAge && Categories[i].MinAge <= Categories[j].MaxAge)
                    {
                        throw new ArgumentException($"Overlapping age ranges detected: {Categories[i].Name} and {Categories[j].Name}");
                    }
                }
            }

            Categories.Sort((c1, c2) => c1.MinAge.CompareTo(c2.MinAge));
            for (int i = 0; i < Categories.Count - 1; i++)
            {
                if (Categories[i].MaxAge + 1 < Categories[i + 1].MinAge)
                {
                    throw new ArgumentException($"Gap detected between age ranges: {Categories[i].Name} and {Categories[i + 1].Name}");
                }
            }

            if (Categories.Any(c => c.MinAge > c.MaxAge))
            {
                throw new ArgumentException("Invalid age category configuration: MinAge cannot be greater than MaxAge.");
            }

            if (Categories.Any(c => c.MinAge < 0 || c.MaxAge < 0))
            {
                throw new ArgumentException("Invalid age category configuration: Age bounds cannot be negative");

            }
        }
    }
}
