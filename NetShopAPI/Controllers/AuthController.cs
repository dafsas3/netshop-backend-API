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
using NetShopAPI.Services.AuthServices.RegisterServices;
using NetShopAPI.Services.AuthServices.LoginServices;
using NetShopAPI.Services.JwtServices;
using NetShopAPI.DTOs.AuthDTO;
using NetShopAPI.Services.ClientServices;
using NetShopAPI.Common;

namespace NetShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IRegisterService _registerService;
        private readonly ILoginService _loginService;
        private readonly IUserAccountService _userAccountService;


        public AuthController(IPasswordHasher<User> hasher,
            IRegisterService registerService, ILoginService loginService,
            IUserAccountService userAccountService)
        {
            _registerService = registerService;
            _loginService = loginService;
            _userAccountService = userAccountService;
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest req, CancellationToken ct)
        {
            var result = await _registerService.CreateUser(req, ct);

            return this.ToActionResult(result);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest req, CancellationToken ct)
        {
            var result = await _loginService.TryAuthorizationUser(req);

            return this.ToActionResult(result);
        }


        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me(CancellationToken ct)
        {
            var result = await _userAccountService.ShowMeAccount(ct);

            return this.ToActionResult(result);
        }


    }
}
