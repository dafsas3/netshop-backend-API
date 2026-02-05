using FluentAssertions;
using NetShopAPI.DTOs.PositionsItemDTO;
using NetShopAPI.Services.PositionProductsServices;
using NetShopAPI.Tests.Infrastructure.DbFactory;
using NetShopAPI.Tests.TestDataFactory;


namespace NetShopAPI.Tests.ServicesTests.IntegrationTests
{
    public class GetPositionProductById_UnitTests
    {


        [Fact]
        public async Task GetPositionById_WhenPositionExists_ShouldReturnData()
        {
            // Arrange
            var (db, conn) = await SqliteInMemoryDbFactory.CreateDbAsync();
            await using var _ = conn;
            await using var __ = db;

            var (category, product, position) = PositionTestDataFactory.CreateCategoryProductPosition(initAmount: 1);
            await PositionTestDataFactory.SeedAsync(db, category, product, position);

            var service = new ProductService(db);

            // Act
            var result = await service.GetPositionProductByIdAsync(position.Id, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();
            result.Error.Should().BeNull();

            result.Data.Should().BeEquivalentTo(new PositionResponse
            {
                Name = position.Name,
                AdditionalInformation = position.AdditionalInformation,
                Price = position.Price,
                CategoryName = category.Name,
                Amount = position.Amount,
                Id = position.Id,
                TotalPrice = position.Price * position.Amount
            });
        }

    }
}
