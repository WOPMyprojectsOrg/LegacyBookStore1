using System.ComponentModel.DataAnnotations;

namespace LegacyBookStore.Models
{
    public class CreateBookRequest
    {
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;
        [StringLength(100)]
        public string Author { get; set; } = string.Empty;
        [Range(0, 10000)]
        public decimal Price { get; set; }
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
    }
}