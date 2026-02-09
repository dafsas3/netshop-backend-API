using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace NetShopAPI.Infrastructure.Persistence
{
    public static class DbUpdateExceptionExtensions
    {

        public static bool IsUniqueViolation(this DbUpdateException ex)
            => ex.InnerException is MySqlException mySql && mySql.Number == 1062;


        public static string? GetUniqueIndexName(this DbUpdateException ex)
        {
            if (ex.InnerException is not MySqlException mySql) return null;

            var exMsg = mySql.Message;

            var markerExMsg = " for key '";

            var i = exMsg.IndexOf(markerExMsg, StringComparison.OrdinalIgnoreCase);
            if (i < 0) return null;

            i += markerExMsg.Length;

            var j = exMsg.IndexOf('\'', i);
            if (j < 0) return null;

            return exMsg.Substring(i, j - i);
        }

    }
}
