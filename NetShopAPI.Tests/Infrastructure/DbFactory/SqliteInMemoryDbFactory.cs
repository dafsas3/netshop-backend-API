using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NetShopAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetShopAPI.Tests.Infrastructure.DbFactory
{
    internal class SqliteInMemoryDbFactory
    {

        public static async Task<(ShopDbContext db, SqliteConnection conn)> CreateDbAsync()
        {
            var conn = new SqliteConnection("DataSource=:memory:");
            await conn.OpenAsync();

            var options = new DbContextOptionsBuilder<ShopDbContext>()
                .UseSqlite(conn)
                .Options;

            var db = new ShopDbContext(options);
            await db.Database.EnsureCreatedAsync();

            return (db, conn);
        } 

    }
}
