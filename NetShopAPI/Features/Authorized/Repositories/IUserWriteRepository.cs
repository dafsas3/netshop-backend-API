using NetShopAPI.Models;

namespace NetShopAPI.Features.Authorized.Repositories
{
    public interface IUserWriteRepository
    {
        Task<User?> GetUserByNickName(string nickName, CancellationToken ct);
        Task<User?> GetUserByLogin(string email, string phone, CancellationToken ct);
        void Add(User user);
    }
}