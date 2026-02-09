using NetShopAPI.Common;
using NetShopAPI.DTOs.CatalogDTO.Categoryes.Response;
using NetShopAPI.Features.Abstractions.Persistence;
using NetShopAPI.Features.Catalog.Repositories.CategoryRepositories;
using NetShopAPI.Models;

namespace NetShopAPI.Features.Catalog.Categories.Command
{
    public class CreateCategoryHandler
    {

        private readonly IUnitOfWork _uow;
        private readonly ICategoryWriteRepository _category;

        public CreateCategoryHandler(IUnitOfWork uow, ICategoryWriteRepository category)
        {
            _uow = uow;
            _category = category;
        }



        public async Task<Result<CategoryResponse>> Handle(CreateCategoryCommand cmd, CancellationToken ct)
        {
            var category = await _category.GetCategoryEntityByNameAsync(cmd, ct);

            if (category is not null)
                return Result<CategoryResponse>.Conflict(
                    "CATEGORY_ALREADY_EXISTS", "Категория с таким названием уже существует.");

            var newCategory = new Category
            {
                Name = cmd.Name
            };

            _category.Add(newCategory);
            await _uow.SaveChangesAsync(ct);

            return Result<CategoryResponse>.Created(new CategoryResponse
            {
                Id = newCategory.Id,
                Name = newCategory.Name
            });
        }
    }
}