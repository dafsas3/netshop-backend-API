using Microsoft.EntityFrameworkCore;
using NetShopAPI.Data;
using NetShopAPI.Features.Authorized.Repositories;
using NetShopAPI.Models;

namespace NetShopAPI.Infrastructure.Persistence.Repositories.UserRepositories
{
    public class EfUserWriteRepository : IUserWriteRepository
    {

        private readonly ShopDbContext _db;

        public EfUserWriteRepository(ShopDbContext db) => _db = db;



        public async Task<User?> GetUserByNickName(string nickName, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(nickName)) return null;

            var normalizedNickName = nickName.Trim();

            return await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.NickName == normalizedNickName, ct);
        }


        public async Task<User?> GetUserByLogin(string email, string phone, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(phone)) return null;

            var normalizeEmail = string.IsNullOrWhiteSpace(email) ? null : email.Trim().ToLowerInvariant();
            var normalizePhone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim();

            return await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => 
                (normalizePhone != null && u.Phone == normalizePhone) ||
                (normalizeEmail != null && u.Email == normalizeEmail), ct);
        }


        public void Add(User user)
        {
            _db.Users.Add(user);
        }

    }
}
