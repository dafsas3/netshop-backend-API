using NetShopAPI.DTOs.CatalogDTO.Products.Request;
using NetShopAPI.Models.GeneralWarehouseModel.PositionItem;
using NetShopAPI.Models;
using NetShopAPI.DTOs.PositionsItemDTO;
using Azure.Core.GeoJson;

namespace NetShopAPI.Services.PositionProductsServices
{
    public static class PositionServiceManager
    {



        public static Product GetCreationProduct(ProductRequest prod)
        {
            return new Product
            {
                Id = new Random().Next(0, 1000000),
                Name = prod.Name,
                Description = prod.Description,
                Price = prod.Price
            };
        }


        public static Position GetCreationPosition(Product newProduct, ProductRequest prod, Category category)
        {
            return new Position
            {
                Id = Guid.NewGuid(),
                Name = newProduct.Name,
                Price = newProduct.Price,
                LastPurchasePrice = newProduct.Price,
                Amount = 0,
                AdditionalInformation = newProduct.Description,
                ProductId = newProduct.Id,
                CategoryId = prod.CategoryId,
                CategoryName = category.Name
            };
        }


        public static BulkPositionResponse GetCreationResponse(Position newPosition, Category category)
        {
            return new BulkPositionResponse
            {
                Id = newPosition.Id,
                Name = newPosition.Name,
                Price = newPosition.Price,
                LastPurchasePrice = newPosition.LastPurchasePrice,
                Amount = newPosition.Amount,
                AdditionalInformation = newPosition.AdditionalInformation,
                TotalPrice = newPosition.Price * newPosition.Amount,
                CategoryName = category.Name
            };
        }


    }
}
