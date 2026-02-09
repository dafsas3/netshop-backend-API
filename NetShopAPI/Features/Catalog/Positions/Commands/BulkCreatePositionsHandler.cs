using NetShopAPI.Common;
using NetShopAPI.DTOs.PositionsItemDTO;
using NetShopAPI.Features.Abstractions.Persistence;
using NetShopAPI.Models;
using NetShopAPI.Models.GeneralWarehouseModel.PositionItem;
using NetShopAPI.Features.Catalog.Repositories.CategoryRepositories;
using NetShopAPI.Features.Catalog.Repositories.ProductPositionRepository.CommandRepositories;
using NetShopAPI.Features.Catalog.Common.CentralizedErrorMessage;

namespace NetShopAPI.Features.Catalog.Commands.BulkCreatePositions
{
    public class BulkCreatePositionsHandler
    {

        private readonly IUnitOfWork _uow;
        private readonly IPositionWriteRepository _positions;
        private readonly ICategoryWriteRepository _categories;
        private readonly IPositionUniquenessReadRepository _uniqRead;

        public BulkCreatePositionsHandler(IUnitOfWork uow, IPositionWriteRepository positions,
            ICategoryWriteRepository categories, IPositionUniquenessReadRepository uniqRead)
        {
            _uow = uow;
            _positions = positions;
            _categories = categories;
            _uniqRead = uniqRead;
        }


        public async Task<Result<List<BulkPositionResponse>>> Handle(
            List<BulkCreatePositionsCommand> cmd, CancellationToken ct)
        {
            var incomingNameCounts = NormalizeIncomingNames(cmd.Select(n => n.Name).ToList());

            var validNames = NormalizeNamesForSql(cmd.Select(n => n.Name).ToList());
            var existsNames = await _uniqRead.GetExistingNamesAsync(validNames, ct);

            var categoryIds = cmd.Select(c => c.CategoryId).Distinct().ToList();
            var existsCategory = await _categories.GetCategoriesByIdAsync(categoryIds, ct);

            var totalResponse = new List<BulkPositionResponse>(cmd.Count);
            var createdPositions = new List<Position>();
            var successRespIndexes = new List<int>();

            foreach (var position in cmd)
            {
                var response = new BulkPositionResponse();

                if (!IsValidData(position, response, existsNames, existsCategory, incomingNameCounts, out var category))
                {
                    response.IsSuccess = false;
                    totalResponse.Add(response);
                    continue;
                }

                var newProduct = CreateProduct(position);
                var newPosition = CreatePosition(newProduct, category!, position);

                createdPositions.Add(newPosition);

                response.IsSuccess = true;
                totalResponse.Add(response);
                successRespIndexes.Add(totalResponse.Count - 1);
            }

            if (createdPositions.Any())
            {
                await _positions.AddRangeAsync(createdPositions, ct);
                await _uow.SaveChangesAsync(ct);

                for (int i = 0; i < createdPositions.Count; i++)
                {
                    var currentPosition = createdPositions[i];
                    var successIndex = successRespIndexes[i];

                    FillSuccessResponse(totalResponse[successIndex], currentPosition);
                }
            }

            return Result<List<BulkPositionResponse>>.Ok(totalResponse);
        }


        private static void FillSuccessResponse(BulkPositionResponse response, Position position)
        {
            response.PositionId = position.Id;
            response.ProductId = position.ProductId;
            response.Name = position.Name;
            response.Amount = position.Amount;
            response.Price = position.Price;
            response.AdditionalInformation = position.AdditionalInformation;
            response.TotalPrice = position.Amount * position.Price;
        }


        private static Position CreatePosition(Product product, Category category, BulkCreatePositionsCommand cmd)
        {
            return new Position
            {
                Name = product.Name,
                Price = product.Price,
                Amount = cmd.Amount,
                AdditionalInformation = product.Description,
                Category = category,
                CategoryName = category.Name,
                Product = product
            };
        }


        private static Product CreateProduct(BulkCreatePositionsCommand cmd)
        {
            return new Product
            {
                Name = cmd.Name,
                Description = cmd.Description,
                Price = cmd.Price
            };
        }


        private static bool IsValidData(
            BulkCreatePositionsCommand dataPosition, BulkPositionResponse response,
            HashSet<string> existsNames, Dictionary<int, Category> existsCategoryes,
            Dictionary<string, int> incomingNameCounts, out Category? category)
        {
            category = null;
            var normalizeName = dataPosition.Name.Trim();

            if (incomingNameCounts[normalizeName] > 1)
            {
                response.ErrorsAddPosition.Add(new BulkPositionResponse.Errors(
                    CatalogOperationsErrors.DuplicateInRequest.Code,
                    CatalogOperationsErrors.DuplicateInRequest.Message + $"{dataPosition.Name}"));
            }

            if (!existsCategoryes.TryGetValue(dataPosition.CategoryId, out var getCategory))
            {
                response.ErrorsAddCategory.Add(new BulkPositionResponse.ErrorsCategories(
                    CatalogOperationsErrors.CategoryNotFound.Code,
                    CatalogOperationsErrors.CategoryNotFound.Message + $"{dataPosition.CategoryId}"));
            }

            else category = getCategory;

            if (existsNames.Contains(dataPosition.Name))
            {
                response.ErrorsAddPosition.Add(new BulkPositionResponse.Errors(
                    CatalogOperationsErrors.PositionAlreadyExists.Code,
                    CatalogOperationsErrors.PositionAlreadyExists.Message + $"{dataPosition.Name}"));
            }

            return !response.ErrorsAddPosition.Any() && !response.ErrorsAddCategory.Any();
        }


        private static List<string> NormalizeNamesForSql(IEnumerable<string?> names) =>
            names
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .Select(n => n.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();


        private static Dictionary<string, int> NormalizeIncomingNames(IEnumerable<string?> names) =>
            names
            .Where(n => !string.IsNullOrWhiteSpace(n))
                .Select(n => n.Trim())
                .GroupBy(g => g, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.Count(), StringComparer.OrdinalIgnoreCase);

    }
}
