using Microsoft.AspNetCore.Identity;
using NetShopAPI.Common;
using NetShopAPI.DTOs.AuthDTO;
using NetShopAPI.Models;

namespace NetShopAPI.Services.AuthServices.LoginServices
{
    public interface ILoginService
    {
        Task<Result<string>> TryAuthorizationUser(LoginRequest req, CancellationToken ct);
        bool TryAuthorizationByPassword(User user, LoginRequest req);
    }
}
