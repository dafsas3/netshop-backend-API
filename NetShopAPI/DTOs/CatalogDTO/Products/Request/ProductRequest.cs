using System.ComponentModel.DataAnnotations;

namespace NetShopAPI.DTOs.CatalogDTO.Products.Request
{
    public class ProductRequest
    {
        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [MinLength(30)]
        [MaxLength(50000)]
        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
