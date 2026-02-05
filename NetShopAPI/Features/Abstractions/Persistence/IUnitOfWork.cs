using NetShopAPI.Models.GeneralWarehouseModel.PositionItem;

namespace NetShopAPI.Features.Abstractions.Persistence
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken ct);       
    }
}
