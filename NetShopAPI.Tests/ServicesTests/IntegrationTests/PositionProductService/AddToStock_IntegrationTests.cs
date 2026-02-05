using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetShopAPI.Services.PositionProductsServices;
using NetShopAPI.Tests.Infrastructure.DbFactory;
using NetShopAPI.Tests.TestDataFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetShopAPI.Tests.ServicesTests.IntegrationTests.PositionProductService
{
    public class AddToStock_IntegrationTests
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

            result.IsSuccess.Should().BeTrue();
            result.Data.Should().NotBeNull();

            result.Data.Should().BeEquivalentTo(new
            {
                PositionId = position.Id,
                ProductId = product.Id,
                position.Name,
                Amount = expectedAmount
            });

            var updated = await db.Positions.FirstAsync(p => p.ProductId == product.Id);

            updated.Amount.Should().Be(expectedAmount);
        }



    }
}
