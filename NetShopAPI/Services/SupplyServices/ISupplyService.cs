using NetShopAPI.DTOs.PositionsItemDTO;
using NetShopAPI.DTOs.SupplyDTO.Request;
using NetShopAPI.DTOs.SupplyDTO.Response;
using NetShopAPI.DTOs.SupplyDTO.Supply_LogDTO;

namespace NetShopAPI.Services.SupplyServices
{
    public interface ISupplyService
    {
        Task<SupplyResponse> AddSupply(SupplyRequest req);
        Task<List<PositionResponse>> GetPositions();
        Task<List<SupplyLogViewResponse>> GetSupplyLogs();
    }
}
