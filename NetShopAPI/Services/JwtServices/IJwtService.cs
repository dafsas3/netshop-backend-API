using NetShopAPI.Models;

namespace NetShopAPI.Services.JwtServices
{
    public interface IJwtService
    {
        Task<string> CreateJwt(User user);
    }
}
