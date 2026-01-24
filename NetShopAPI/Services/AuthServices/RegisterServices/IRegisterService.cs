using NetShopAPI.Common;
using NetShopAPI.DTOs.AuthDTO;
using NetShopAPI.DTOs.User.ResponseDTO;
using NetShopAPI.Models;

namespace NetShopAPI.Services.AuthServices.RegisterServices
{
    public interface IRegisterService
    {
        string GetUserEmail(RegisterRequest request);
        Task<bool> IsExistsUserEmailAsync(string email);
        Task<bool> IsExistsUserPhoneNumber(string Phone);
        Task<Result<UserResponse>> IsExistsUserEmailOrPhone(string Phone, string email);
        Task<Result<UserResponse>> CreateUser(RegisterRequest req);
    }
}
