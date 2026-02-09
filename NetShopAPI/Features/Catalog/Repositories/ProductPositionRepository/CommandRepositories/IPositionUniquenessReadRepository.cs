namespace NetShopAPI.Features.Catalog.Repositories.ProductPositionRepository.CommandRepositories
{
    public interface IPositionUniquenessReadRepository
    {
        Task<HashSet<string>> GetExistingNamesAsync(IEnumerable<string> names, CancellationToken ct);
    }
}
