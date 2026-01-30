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
using NetShopAPI.Common;
using NetShopAPI.Services.JwtServices;

namespace NetShopAPI.Services.AuthServices.LoginServices
{
    public class LoginService : ILoginService
    {

        private readonly IPasswordHasher<User> _hasher;
        private readonly ShopDbContext _db;
        private readonly IJwtService _jwtService;

        public LoginService(ShopDbContext db, IPasswordHasher<User> hasher, IJwtService jwtService)
        {
            _db = db;
            _hasher = hasher;
            _jwtService = jwtService;
        }


        public async Task<Result<string>> TryAuthorizationUser(LoginRequest req, CancellationToken ct)
        {
            var login = req.Login.Trim().ToLowerInvariant();

            var user = await _db.Users.FirstOrDefaultAsync(x =>
                x.Email == login || (x.Phone != null && x.Phone == req.Login.Trim()), ct);

            if (user is null)
                return Result<string>.Unauthorized("INVALID_CREDENTIALS", "Неверный логин или пароль!");

            if (!TryAuthorizationByPassword(user, req))
                return Result<string>.Unauthorized("INVALID_CREDENTIALS", "Неверный логин или пароль!");

            var token = _jwtService.CreateJwt(user);
            return Result<string>.Ok(token);
        }


        public bool TryAuthorizationByPassword(User user, LoginRequest req)
        {
            var verify = _hasher.VerifyHashedPassword(user, user.PasswordHash, req.Password);

            if (verify is PasswordVerificationResult.Failed) return false;

            return true;
        }


    }
}
