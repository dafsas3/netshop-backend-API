using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NetShopAPI.Data;
using NetShopAPI.Models;
using NetShopAPI.Models.GeneralWarehouseModel.PositionItem;
using NetShopAPI.Services.PositionProductsServices;
using NetShopAPI.Tests.Infrastructure.DbFactory;
using NetShopAPI.Tests.TestDataFactory;
using Xunit;
using FluentAssertions;

namespace NetShopAPI.Tests.ServicesTests.IntegrationTests.PositionProductService
{

    public class ProductServiceSqliteInMemoryTests
    {

      


        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task AddToStock_When_quantity_is_zero_or_negative_Should_return_BadRequest(int quantity)
        {
            int testProductId = 1;

            var (db, conn) = await SqliteInMemoryDbFactory.CreateDbAsync();
            await using var _ = conn;
            await using var __ = db;

            var service = new ProductService(db);

            var result = await service.AddToStock(testProductId, quantity, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Data.Should().BeNull();

            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("INVALID_QUANTITY");
        }


        [Fact]
        public async Task AddToStock_When_Product_is_null_return_NotFound()
        {
            int testProductId = 1;

            var (db, conn) = await SqliteInMemoryDbFactory.CreateDbAsync();
            await using var _ = conn;
            await using var __ = db;

            var service = new ProductService(db);

            var result = await service.AddToStock(testProductId, quantity: 1, CancellationToken.None);

            result.IsSuccess.Should().BeFalse();
            result.Data.Should().BeNull();

            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("INVALID_PRODUCT_ID");

            (await db.Positions.AnyAsync()).Should().BeFalse();
        }


    }
}
