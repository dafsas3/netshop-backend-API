using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetShopAPI.Common;
using NetShopAPI.DTOs.User.ResponseDTO;
using NetShopAPI.Features.Abstractions.Persistence;
using NetShopAPI.Features.Authorized.Common.CentralizedErrorMessage;
using NetShopAPI.Features.Authorized.Repositories;
using NetShopAPI.Infrastructure.Persistence;
using NetShopAPI.Models;

namespace NetShopAPI.Features.Authorized.Register.Commands
{
    public class CreateRegisterUserHandler
    {

        private readonly IUnitOfWork _uow;
        private readonly IUserWriteRepository _users;
        private readonly IPasswordHasher<User> _hasher;

        public CreateRegisterUserHandler(IUnitOfWork uow, IUserWriteRepository users, IPasswordHasher<User> hasher)
        {
            _uow = uow;
            _users = users;
            _hasher = hasher;
        }



        public async Task<Result<UserResponse>> Handle(CreateRegisterUserCommand cmd, CancellationToken ct)
        {
            if (await _users.GetUserByNickName(cmd.NickName, ct) is not null)
                return Result<UserResponse>.Conflict(
                    AuthorizedOperationsErrors.NicknameAlreadyAxists.Code,
                    AuthorizedOperationsErrors.NicknameAlreadyAxists.Message);

            if (await _users.GetUserByLogin(cmd.Email, cmd.Phone, ct) is not null)
                return Result<UserResponse>.Conflict(
                    AuthorizedOperationsErrors.PhoneOrEmailAlreadyExists.Code,
                    AuthorizedOperationsErrors.PhoneOrEmailAlreadyExists.Message);

            var newUser = CreateUser(cmd);
            newUser.PasswordHash = _hasher.HashPassword(newUser, cmd.Password);

            _users.Add(newUser);

            try
            {
                await _uow.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (ex.IsUniqueViolation())
            {
                var ux = ex.GetUniqueIndexName();

                return ux switch
                {
                    "UX_Users_Email" or "UX_Users_Phone" => Result<UserResponse>.Conflict(
                        AuthorizedOperationsErrors.PhoneOrEmailAlreadyExists.Code,
                        AuthorizedOperationsErrors.PhoneOrEmailAlreadyExists.Message),
                    
                    "UX_Users_NickName" => Result<UserResponse>.Conflict(
                        AuthorizedOperationsErrors.NicknameAlreadyAxists.Code,
                        AuthorizedOperationsErrors.NicknameAlreadyAxists.Message),

                    _ => Result<UserResponse>.Conflict(
                        AuthorizedOperationsErrors.PhoneOrEmailAlreadyExists.Code,
                        AuthorizedOperationsErrors.PhoneOrEmailAlreadyExists.Message)
                };
            }

            return Result<UserResponse>.Created(FillUserResponse(newUser));
        }


        private static UserResponse FillUserResponse(User newUser)
        {
            return new UserResponse
            {
                Id = newUser.Id,
                NickName = newUser.NickName,
                FirstName = newUser.FirstName,
                SurName = newUser.SurName,
                LastName = newUser.LastName,
                Phone = newUser.Phone,
                Email = newUser.Email,
                Role = newUser.Role
            };
        }


        private static User CreateUser(CreateRegisterUserCommand cmd)
        {
            return new User
            {
                FirstName = cmd.FirstName,
                SurName = cmd.SurName,
                LastName = cmd.LastName,
                NickName = cmd.NickName.Trim(),
                Email = cmd.Email.Trim().ToLowerInvariant(),
                Phone = string.IsNullOrWhiteSpace(cmd.Phone) ? null : cmd.Phone.Trim(),
                CreatedAtUtc = DateTime.UtcNow,
                Role = "User"
            };
        }


    }
}
