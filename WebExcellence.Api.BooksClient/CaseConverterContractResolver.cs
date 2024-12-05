using Newtonsoft.Json.Serialization;
using System.Globalization;

namespace WebExcellence.Api.BooksClient
{
    public class CaseConverterContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return base.ResolvePropertyName(propertyName.ToLower(CultureInfo.InvariantCulture));
        }
    }
}
