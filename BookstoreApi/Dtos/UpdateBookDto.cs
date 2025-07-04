using System.ComponentModel.DataAnnotations;

namespace BookstoreApi.Dtos
{
    public class UpdateBookDto
    {
        [StringLength(100)]
        public string? Title { get; set; }

        public List<string>? Authors { get; set; }

        [StringLength(50)]
        public string? Category { get; set; }

        [Range(1000, 2100)]
        public int? Year { get; set; }

        [Range(0.01, 1000.00)]
        public decimal? Price { get; set; }
    }
}