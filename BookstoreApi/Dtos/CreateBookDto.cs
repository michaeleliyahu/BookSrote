using System.ComponentModel.DataAnnotations;

namespace BookstoreApi.Dtos
{
    public class CreateBookDto
    {
        [Required]
        [RegularExpression(@"^\d{10}(\d{3})?$", ErrorMessage = "ISBN must be 10 or 13 digits.")]
        public string Isbn { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public List<string> Authors { get; set; } = new();

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [Range(1000, 2100)]
        public int Year { get; set; }


        [Required]
        [Range(0.01, 1000.00)]
        public decimal Price { get; set; }
    }
}
