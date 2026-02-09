using Microsoft.AspNetCore.Identity;
using NetShopAPI.Common;
using NetShopAPI.DTOs.User.ResponseDTO;
using NetShopAPI.Features.Authorized.Common.CentralizedErrorMessage;
using NetShopAPI.Features.Authorized.Repositories;
using NetShopAPI.Features.Authorized.Services;
using NetShopAPI.Models;

namespace NetShopAPI.Features.Authorized.Login.Query
{
    public class AuthorizationUserHandler
    {

        private readonly IUserWriteRepository _users;
        private readonly IPasswordHasher<User> _hasher;
        private readonly IJwtService _jwt;
        
        public AuthorizationUserHandler(IUserWriteRepository users, IPasswordHasher<User> hasher, IJwtService jwt)
        {
            _users = users;
            _hasher = hasher;
            _jwt = jwt;
        }


        public async Task<Result<string>> Handle(AuthorizationUserCommand cmd, CancellationToken ct)
        {
            var user = await _users.GetUserByLogin(cmd.Email, cmd.Phone, ct);

            if (user is null || !TryVerifyPasswordHash(user, cmd))
                return Result<string>.Unauthorized(
                    AuthorizedOperationsErrors.InvalidCredentials.Code,
                    AuthorizedOperationsErrors.InvalidCredentials.Message);

            var token = _jwt.CreateJwt(user);

            return Result<string>.Ok(token);
        }


        private bool TryVerifyPasswordHash(User user, AuthorizationUserCommand cmd)
        {
            var verify = _hasher.VerifyHashedPassword(user, user.PasswordHash, cmd.Password);

            if (verify is PasswordVerificationResult.Failed) return false;

            return true;
        }

    }
}
