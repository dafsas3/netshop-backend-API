using Microsoft.EntityFrameworkCore;
using NetShopAPI.Data;
using NetShopAPI.DTOs.PositionsItemDTO;
using NetShopAPI.DTOs.SupplyDTO.Request;
using NetShopAPI.DTOs.SupplyDTO.Response;
using NetShopAPI.Models.GeneralWarehouseModel.PositionItem;
using NetShopAPI.Models.SupplyModel;
using NetShopAPI.DTOs.SupplyDTO.Supply_LogDTO;

namespace NetShopAPI.Services.SupplyServices
{
    public class SupplyService(ShopDbContext shopDbContext) : ISupplyService
    {

        private readonly ShopDbContext _db = shopDbContext;



        public async Task<SupplyResponse> AddSupply(SupplyRequest req)
        {
            var response = new SupplyResponse { Name = req.Name };

            var existingPosition = await _db.Positions.Where(p => p.Name == req.Name).FirstOrDefaultAsync();

            SupplyLog supplyLog;
            Guid positionId;

            if (existingPosition is not null)
            {
                existingPosition.Amount += req.Amount;
                existingPosition.LastPurchasePrice = req.Price;
                existingPosition.AdditionalInformation = req.AdditionalInformation;

                positionId = existingPosition.Id;

                supplyLog = MakeWriteLog(existingPosition, req.Amount);
            }

            else
            {
                var newPosition = new Position
                {
                    Name = req.Name,
                    Amount = req.Amount,
                    Price = req.Price,
                    LastPurchasePrice = req.Price,
                    AdditionalInformation = req.AdditionalInformation
                };

                _db.Positions.Add(newPosition);

                positionId = newPosition.Id;

                supplyLog = MakeWriteLog(newPosition, req.Amount);
            }

            response = FillInResponse(response, supplyLog, positionId);

            await _db.SaveChangesAsync();

            return response;
        }


        private SupplyLog MakeWriteLog(Position item, int amount)
        {
            var newSupplyLog = new SupplyLog
            {
                PositionName = item.Name,
                AmountReceived = amount,
                PriceAtSupply = item.Price,
                Date = DateTime.UtcNow,
                AdditionalInformation = item.AdditionalInformation
            };

            _db.SupplyLogs.Add(newSupplyLog);

            return newSupplyLog;
        }


        private SupplyResponse FillInResponse(SupplyResponse response, SupplyLog log, Guid positionId)
        {
            response.Price = log.PriceAtSupply;
            response.Totalprice += (log.PriceAtSupply * (decimal)log.AmountReceived);
            response.AdditionalInformation = log.AdditionalInformation;
            response.Amount += log.AmountReceived;
            response.Id = positionId;

            return response;
        }


        public async Task<List<PositionResponse>> GetPositions()
        {
            var existingPositions = await _db.Positions
                .Where(p => !p.IsDelete)
                .Select(p => new PositionResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Amount = p.Amount,
                    Price = p.Price,
                    LastPurchasePrice = p.LastPurchasePrice,
                    AdditionalInformation = p.AdditionalInformation,
                    TotalPrice = p.Price * p.Amount
                })
                .ToListAsync();

            return existingPositions;
        }


        public async Task<List<SupplyLogViewResponse>> GetSupplyLogs()
        {
            var existingLogs = await _db.SupplyLogs
                .Where(sl => !sl.IsDelete)
                .Select(sl => new SupplyLogViewResponse
                {
                    Id = sl.Id,
                    PositionName = sl.PositionName,
                    AmountReceived = sl.AmountReceived,
                    PriceAtSupply = sl.PriceAtSupply,
                    AdditionalInformation = sl.AdditionalInformation,
                    Date = DateTime.UtcNow
                })
                .ToListAsync();

            return existingLogs;
        }

    }
}
