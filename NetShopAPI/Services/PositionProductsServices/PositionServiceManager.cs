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
                Name = prod.Name,
                Description = prod.Description,
                Price = prod.Price
            };
        }


        public static Position GetCreationPosition(Product newProduct, ProductRequest prod, Category category)
        {
            return new Position
            {
                Name = newProduct.Name,
                Price = newProduct.Price,
                LastPurchasePrice = newProduct.Price,
                Amount = 0,
                AdditionalInformation = newProduct.Description,
                Product = newProduct,
                CategoryId = prod.CategoryId,
                CategoryName = category.Name
            };
        }


    }
}
