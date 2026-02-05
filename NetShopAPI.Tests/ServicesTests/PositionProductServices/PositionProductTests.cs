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
            var expectedAmount = initAmountProduct + addQuantity;

            var (db, conn) = await SqliteInMemoryDbFactory.CreateDbAsync();
            await using var _ = conn;
            await using var __ = db;

            var (category, product, position) = PositionTestDataFactory.CreateCategoryProductPosition(initAmountProduct);
            await PositionTestDataFactory.SeedAsync(db, category, product, position);

            var service = new ProductService(db);

            var result = await service.AddToStock(product.Id, addQuantity, CancellationToken.None);

            result.IsSucces.Should().BeTrue();
            result.Data.Should().NotBeNull();

            result.Data.Should().BeEquivalentTo(new
            {
                PositionId = position.Id,
                ProductId = product.Id,
                Name = position.Name,
                Amount = expectedAmount
            });

            var updated = await db.Positions.FirstAsync(p => p.ProductId == product.Id);

            updated.Amount.Should().Be(expectedAmount);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task AddToStock_When_quantity_is_zero_or_negative_Should_return_BadRequest(int quantity)
        {
            int expected = 1;
            int initTestPositionAmount = 1;

            var (db, conn) = await SqliteInMemoryDbFactory.CreateDbAsync();
            await using var _ = conn;
            await using var __ = db;

            var (category, product, position) = PositionTestDataFactory.CreateCategoryProductPosition(initTestPositionAmount);

            await PositionTestDataFactory.SeedAsync(db, category, product, position);

            var service = new ProductService(db);

            var result = await service.AddToStock(product.Id, quantity, CancellationToken.None);

            result.IsSucces.Should().BeFalse();
            result.Data.Should().BeNull();

            result.Error.Should().NotBeNull();
            result.Error.Code.Should().Be("INVALID_QUANTITY");

            var updated = await db.Positions.FirstAsync(p => p.ProductId == product.Id);
            updated.Amount.Should().Be(expected);
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
