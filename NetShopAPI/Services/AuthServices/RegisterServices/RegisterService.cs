using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using NetShopAPI.Data;
using NetShopAPI.Models;
using NetShopAPI.DTOs.AuthDTO;
using NetShopAPI.DTOs.User.ResponseDTO;
using NetShopAPI.Common;

namespace NetShopAPI.Services.AuthServices.RegisterServices
{
    public class RegisterService : IRegisterService
    {

        private readonly ShopDbContext _db;
        private readonly IPasswordHasher<User> _hasher;


        public RegisterService(ShopDbContext db, IPasswordHasher<User> hasher)
        {
            _db = db;
            _hasher = hasher;
        }




        public string GetUserEmail(RegisterRequest request)
        {
            var email = request.Email.Trim().ToLowerInvariant();
            return email;
        }


        public async Task<bool> IsExistsUserEmailAsync(string email)
        {
            if (await _db.Users.AnyAsync(x => x.Email == email))
                return true;

            return false;
        }


        public async Task<bool> IsExistsUserPhoneNumber(string Phone)
        {
            var phone = Phone.Trim();
            if (await _db.Users.AnyAsync(x => x.Phone == phone))
                return true;

            return false;
        }


        public async Task<Result<UserResponse>> IsExistsUserEmailOrPhone(string Phone, string email)
        {
            if (await IsExistsUserEmailAsync(email))
                return Result<UserResponse>.Conflict("EMAIL_ALREADY_EXISTS",
                    "Данный email уже используется!");

            if (!string.IsNullOrWhiteSpace(Phone) &&
                await IsExistsUserPhoneNumber(Phone))
                return Result<UserResponse>.Conflict("PHONE_ALREADY_EXISTS",
                    "Данный номер телефона уже используется!");

            return Result<UserResponse>.OkWithoutData();
        }


        public async Task<Result<UserResponse>> CreateUser(RegisterRequest req)
        {
            var email = GetUserEmail(req);

            var check = await IsExistsUserEmailOrPhone(req.Phone, email);

            if (!check.IsSucces) return check;

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = req.FirstName.Trim(),
                LastName = req.LastName.Trim(),
                SurName = req.SurName.Trim(),
                NickName = req.NickName.Trim(),
                Email = email,
                Phone = string.IsNullOrWhiteSpace(req.Phone) ? null : req.Phone.Trim(),
                CreatedAtUtc = DateTime.UtcNow,
                Role = "User"
            };

            user.PasswordHash = GetHashUserPassword(user, req);

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            var response = new UserResponse
            {
                Id = user.Id,   
                FirstName = user.FirstName,
                LastName = user.LastName,
                SurName = user.SurName,
                NickName = user.NickName,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role
            };

            return Result<UserResponse>.Created(response);
        }


        public string GetHashUserPassword(User user, RegisterRequest req)
        {
            var passwordHash = _hasher.HashPassword(user, req.Password);
            return passwordHash;
        }


    }
}
