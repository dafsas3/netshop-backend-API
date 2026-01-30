using Microsoft.AspNetCore.Mvc;
using NetShopAPI.Common;
using NetShopAPI.DTOs.User.ResponseDTO;
using System.Security.Claims;

namespace NetShopAPI.Services.ClientServices
{
    public interface IUserAccountService
    {
        Task<Result<UserResponse>> TryGetUserByIdAsync(Guid userId, CancellationToken ct);
        Task<Result<UserResponse>> ShowMeAccount(CancellationToken ct);
    }
}
