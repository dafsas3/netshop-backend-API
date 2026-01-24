using System.ComponentModel.DataAnnotations;

namespace NetShopAPI.DTOs.CatalogDTO.Categoryes.Request
{
    public class CategoryRequest
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }
    }
}
