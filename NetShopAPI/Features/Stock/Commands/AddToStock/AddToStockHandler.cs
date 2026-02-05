using NetShopAPI.Features.Stock.DTOs;
using NetShopAPI.Common;
using NetShopAPI.Models;
using NetShopAPI.Features.Abstractions.Persistence;

namespace NetShopAPI.Features.Stock.Commands.AddToStock
{
    public class AddToStockHandler
    {

        private readonly IPositionRepository _positions;
        private readonly IUnitOfWork _uow;

        public AddToStockHandler(IPositionRepository positions, IUnitOfWork uow)
        {
            _positions = positions;
            _uow = uow;
        }


        public async Task<Result<PositionAddStockResponse>> Handle(AddToStockCommand command, CancellationToken ct)
        {
            if (command.Quantity <= 0)
                return Result<PositionAddStockResponse>.BadRequest(
                "INVALID_QUANTITY",
                $"Введённое количество добавляемого продукта не может быть: {command.Quantity}");

            var position = await _positions.GetByProductIdAsync(command.ProductId, ct);

            if (position is null)
                return Result<PositionAddStockResponse>.NotFound(
                 "INVALID_PRODUCT_ID",
                 $"ProductID: {command.ProductId} не найден в базе данных!");

            position.Amount += command.Quantity;
            await _uow.SaveChangesAsync(ct);

            return Result<PositionAddStockResponse>.Ok(new PositionAddStockResponse
            {
                ProductId = position.ProductId,
                PositionId = position.Id,
                Amount = position.Amount,
                Name = position.Name
            });
        }

    }
}
