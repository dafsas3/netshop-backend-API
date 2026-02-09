using Microsoft.EntityFrameworkCore;
using NetShopAPI.Common;
using NetShopAPI.Data;
using NetShopAPI.DTOs.CatalogDTO.Categoryes.Response;

namespace NetShopAPI.Features.Catalog.Categories.Query
{
    public class GetAllCategoriesHandler
    {

        private readonly ShopDbContext _db;

        public GetAllCategoriesHandler(ShopDbContext db) => _db = db;


        public async Task<Result<List<CategoryResponse>>> Handle(CancellationToken ct)
        {
            var existingCategoriesDto = await _db.ProductCategories
               .Select(c => new CategoryResponse
               {
                   Id = c.Id,
                   Name = c.Name!
               }).ToListAsync(ct);

            return Result<List<CategoryResponse>>.Ok(existingCategoriesDto);
        }

    }
}
