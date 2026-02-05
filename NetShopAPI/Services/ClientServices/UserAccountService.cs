using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetShopAPI.Common;
using NetShopAPI.Data;
using NetShopAPI.DTOs.User.ResponseDTO;
using NetShopAPI.Services.CurrentUserServices;
using System.Security.Claims;

namespace NetShopAPI.Services.ClientServices
{
    public class UserAccountService : IUserAccountService
    {

        private readonly ShopDbContext _db;
        private readonly ICurrentUserService _currentUser;

        public UserAccountService(ShopDbContext db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }



        public async Task<Result<UserResponse>> ShowMeAccount(CancellationToken ct)
        {
            if (!_currentUser.IsAuthenticated || _currentUser.UserId is null)
                return Result<UserResponse>.Unauthorized("UNAUTHORIZED", "Требуется авторизация!");

            var userResult = await TryGetUserByIdAsync(_currentUser.UserId.Value, ct);

            if (!userResult.IsSuccess)
                return Result<UserResponse>.NotFound("USER_NOT_FOUND", "Пользователь не найден в базе данных.");

            return Result<UserResponse>.Ok(userResult.Data);
        }


        public async Task<Result<UserResponse>> TryGetUserByIdAsync(Guid userId, CancellationToken ct)
        {
            var user = await _db.Users
                .Where(x => x.Id == userId)
                .Select(x => new UserResponse
                {
                    Id = userId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SurName = x.SurName,
                    NickName = x.NickName,
                    Email = x.Email,
                    Phone = x.Phone,
                    Role = x.Role
                })
                .FirstOrDefaultAsync(ct);

            return user is not null
                 ? Result<UserResponse>.Ok(user)
                 : Result<UserResponse>.NotFound("USER_NOT_FOUND", "Пользователь не найден в базе данных.");
        }


    }
}
