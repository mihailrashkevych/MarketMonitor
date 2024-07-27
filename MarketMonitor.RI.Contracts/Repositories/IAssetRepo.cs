using MarketMonitor.RI.Contracts.DbEntities;
using MarketMonitor.RI.Contracts.Entities;

namespace MarketMonitor.RI.Contracts.Repositories
{
    public interface IAssetRepo
    {
        Task<IEnumerable<DbAsset>> GetAssetsAsync(string id = null, string symbol = null, string currency = null, string provider = null);
        Task AddAssetsAsync(IEnumerable<Asset> assets);
        IEnumerable<DbAsset> GetAssetsAsync(Func<DbAsset, bool> predicate);
    }
}
