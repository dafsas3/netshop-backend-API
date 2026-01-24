using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetShopAPI.DTOs.SupplyDTO.Request;
using NetShopAPI.Services.SupplyServices;

namespace NetShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplyController : ControllerBase
    {

        private readonly ISupplyService _supplyService;

        public SupplyController(ISupplyService supplyService)
        {
            _supplyService = supplyService;
        }



        [HttpPost("add-supply")]
        public async Task<IActionResult> CreateSupply(SupplyRequest req)
        {
            var result = await _supplyService.AddSupply(req);
            return Ok(result);
        }


        [HttpGet("get-all-supply")]
        public async Task<IActionResult> GetAllSupply()
        {
            var result = await _supplyService.GetPositions();
            return Ok(result);
        }


        [HttpGet("get-supply-logs")]
        public async Task<IActionResult> GetAllSupplyLogs()
        {
            var result = await _supplyService.GetSupplyLogs();
            return Ok(result);
        }

    }
}
