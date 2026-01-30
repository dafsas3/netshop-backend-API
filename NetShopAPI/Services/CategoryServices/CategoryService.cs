using Microsoft.EntityFrameworkCore;
using NetShopAPI.Common;
using NetShopAPI.Data;
using NetShopAPI.DTOs.CatalogDTO.Categoryes.Request;
using NetShopAPI.DTOs.CatalogDTO.Categoryes.Response;
using NetShopAPI.Models;

namespace NetShopAPI.Services.CatalogServices
{
    public class CategoryService : ICategoryService
    {

        private readonly ShopDbContext _db;

        public CategoryService(ShopDbContext db)
        {
            _db = db;
        }


        public async Task<Result<CategoryResponse>> CreateCategoryAsync(CategoryRequest req, CancellationToken ct)
        {
            if (await _db.ProductCategories.AnyAsync(c => c.Name == req.Name, ct))
                return Result<CategoryResponse>.Conflict("CATEGORY_ALREADY_EXISTS",
                    "Данная категория уже существует в базе данных!");

            var category = new Category
            {
                Name = req.Name
            };

            _db.ProductCategories.Add(category);
            await _db.SaveChangesAsync(ct);

            var response = new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name!
            };

            return Result<CategoryResponse>.Created(response);
        }


        public async Task<List<CategoryResponse>> GetAllAsync(CancellationToken ct)
        {
            return await _db.ProductCategories.Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name!
            }).ToListAsync(ct);
        }

    }
}
