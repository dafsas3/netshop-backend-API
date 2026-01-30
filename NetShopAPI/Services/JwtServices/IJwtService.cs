using NetShopAPI.Models;

namespace NetShopAPI.Services.JwtServices
{
    public interface IJwtService
    {
        string CreateJwt(User user);
    }
}
