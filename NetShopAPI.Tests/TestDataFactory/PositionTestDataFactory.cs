using NetShopAPI.Data;
using NetShopAPI.Models;
using NetShopAPI.Models.GeneralWarehouseModel.PositionItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetShopAPI.Tests.TestDataFactory
{
    internal class PositionTestDataFactory
    {

        public static (Category category, Product product, Position position)
            CreateCategoryProductPosition(int initAmount)
        {
            var category = new Category { Name = "Тест-Товары" };
            var product = new Product { Name = "Печенье", Description = "Очень вкусное", Price = 25m };

            var position = new Position
            {
                Product = product,
                Price = product.Price,
                Name = product.Name,
                Amount = initAmount,
                Category = category,
                AdditionalInformation = "Информация о позиции: ..."
            };

            return (category, product, position);
        }


        public static async Task SeedAsync(ShopDbContext db, Category c, Product p, Position pos)
        {
            db.AddRange(c, p, pos);
            await db.SaveChangesAsync();
        }

    }
}
