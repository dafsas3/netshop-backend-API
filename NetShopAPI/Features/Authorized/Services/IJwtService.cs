using NetShopAPI.Models;

namespace NetShopAPI.Features.Authorized.Services
{
    public interface IJwtService
    {
        string CreateJwt(User user);
    }
}
