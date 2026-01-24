using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetShopAPI.Common;
using NetShopAPI.DTOs.Cart.Request;
using NetShopAPI.Services.CartServices;
using System.Security.Claims;

namespace NetShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }



        [HttpGet("get")]
        public async Task<IActionResult> GetUserCartAsync()
        {
            var result = await _cartService.GetUserCartAsync();
            return this.ToActionResult(result);
        }


        [Authorize]
        [HttpPost("add-product")]
        public async Task<IActionResult> AddProductToUserCart(AddToCartRequest req)
        {
            var result = await _cartService.AddProductToUserCart(req);

            return this.ToActionResult(result);
        }

    }
}
