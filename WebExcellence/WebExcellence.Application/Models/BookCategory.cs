using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
