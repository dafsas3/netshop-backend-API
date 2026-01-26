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


        public static void FillSuccessResponse(BulkPositionResponse response, Position position)
        {
            response.PositionId = position.Id;
            response.ProductId = position.ProductId;
            response.Name = position.Name;
            response.Price = position.Price;
            response.Amount = position.Amount;
            response.LastPurchasePrice = position.LastPurchasePrice;
            response.AdditionalInformation = position.AdditionalInformation;
            response.TotalPrice = position.Price * position.Amount;
            response.CategoryName = position.CategoryName;
        }


        public static bool TryValidate(ProductRequest req,
            BulkPositionResponse response,
            Dictionary<int, Category> existingCategories,
            HashSet<string> existingProduct,
            out Category? category)
        {
            category = null;

            if (!existingCategories.TryGetValue(req.CategoryId, out var cat))
            {
                response.ErrorsAddCategory.Add(new
                    BulkPositionResponse.ErrorsCategories(req.CategoryId,
                    "В базе данных не существует ID данной категории."));
            }

            else category = cat;

            if (existingProduct.Contains(req.Name))
            {
                response.ErrorsAddPosition.Add(new BulkPositionResponse.Errors(req.Name,
                    " - Продукт с данным именем уже существует в базе данных."));
            }

            return !response.ErrorsAddCategory.Any() && !response.ErrorsAddPosition.Any();
        }


    }
}
