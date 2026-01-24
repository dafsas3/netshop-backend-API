namespace NetShopAPI.DTOs.CatalogDTO.Products.Response
{
    public class ProductForSearchByIdResponse
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}
