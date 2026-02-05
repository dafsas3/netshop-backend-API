namespace NetShopAPI.Features.Catalog.Commands.BulkCreatePositions
{
    public record BulkCreatePositionsCommand(string Name, string Description, int CategoryId, int Amount, decimal Price);
}
