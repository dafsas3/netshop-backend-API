using NetShopAPI.Features.Catalog.Commands.BulkCreatePositions;

namespace NetShopAPI.Features.Catalog.Positions.DTOs.Request
{
    public sealed record BulkCreatePositionRequest(List<BulkCreatePositionsCommand> Items);
}
