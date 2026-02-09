using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NetShopAPI.DTOs.AuthDTO;
using NetShopAPI.Services.ClientServices;
using NetShopAPI.Common;
using NetShopAPI.Features.Authorized.Register.Commands;
using NetShopAPI.Features.Authorized.Login.Query;

namespace NetShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly CreateRegisterUserHandler _registerHandler;
        private readonly AuthorizationUserHandler _loginHandler;
        // Изменить под cqrs
        private readonly IUserAccountService _userAccountService;


        public AuthController(CreateRegisterUserHandler regHandler, AuthorizationUserHandler logHandler,
            IUserAccountService userAcc)
        {
            _registerHandler = regHandler;
            _loginHandler = logHandler;
            _userAccountService = userAcc;
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateRegisterUserCommand cmd, CancellationToken ct)
        {
            var result = await _registerHandler.Handle(cmd, ct);
            return this.ToActionResult(result);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthorizationUserCommand cmd, CancellationToken ct)
        {
            var result = await _loginHandler.Handle(cmd, ct);
            return this.ToActionResult(result);
        }


        // Изменить под cqrs
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me(CancellationToken ct)
        {
            var result = await _userAccountService.ShowMeAccount(ct);
            return this.ToActionResult(result);
        }


    }
}
