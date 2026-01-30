using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetShopAPI.DTOs.OrderDTO.Request;
using NetShopAPI.Services.OrderServices;
using System.Security.Claims;

namespace NetShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }




        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest req, CancellationToken ct)
        {
            var result = await _orderService.CreateUserOrder(req, ct);

            return Ok(result);
        }


    }
}
