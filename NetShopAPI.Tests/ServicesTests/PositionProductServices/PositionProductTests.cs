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

namespace NetShopAPI.Tests.ServicesTests.PositionProductServices
{

    public class ProductServiceSqliteInMemoryTests
    {

        [Theory]
        [InlineData(2, 5)]
        [InlineData(1, 13)]
        [InlineData(11, 9)]
        public async Task AddToStock_When_position_exists_Should_increase_amount_save_and_return_ok(
            int initAmountProduct, int addQuantity)
        {
            var (db, conn) = await SqliteInMemoryDbFactory.CreateDbAsync();
            await using var _ = conn;
            await using var __ = db;

            var (category, product, position) = PositionTestDataFactory.CreateCategoryProductPosition(initAmountProduct);
            await PositionTestDataFactory.SeedAsync(db, category, product, position);

            var service = new ProductService(db);

            var result = await service.AddToStock(product.Id, addQuantity, CancellationToken.None);

            Assert.True(result.IsSucces);
            Assert.NotNull(result.Data);

            Assert.Equal(position.Id, result.Data.PositionId);
            Assert.Equal(product.Id, result.Data.ProductId);
            Assert.Equal(position.Name, result.Data.Name);
            Assert.Equal(initAmountProduct + addQuantity, result.Data.Amount);

            var updated = await db.Positions.FirstAsync(p => p.ProductId == product.Id);

            Assert.Equal(initAmountProduct + addQuantity, updated.Amount);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task AddToStock_When_quantity_is_zero_or_negative_Should_return_BadRequest(int quantity)
        {
            var (db, conn) = await SqliteInMemoryDbFactory.CreateDbAsync();
            await using var _ = conn;
            await using var __ = db;

            var (category, product, position) = PositionTestDataFactory.CreateCategoryProductPosition(
                initAmount: 1);

            await PositionTestDataFactory.SeedAsync(db, category, product, position);

            var service = new ProductService(db);

            var result = await service.AddToStock(product.Id, quantity, CancellationToken.None);

            Assert.False(result.IsSucces);
            Assert.Null(result.Data);

            Assert.NotNull(result.Error);
            Assert.Equal("INVALID_QUANTITY", result.Error.Code);

            var updated = await db.Positions.FirstAsync(p => p.ProductId == product.Id);
            Assert.Equal(1, updated.Amount);
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

            result.IsSucces.Should().BeFalse();
            result.Data.Should().BeNull();

            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("INVALID_PRODUCT_ID");

            (await db.Positions.AnyAsync()).Should().BeFalse();
        }


    }
}
