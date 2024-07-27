using MarketMonitor.Core.Contracts.Models;
namespace MarketMonitor.Core.Contracts.Services
{
    public interface IAssetService
    {
        Task<IEnumerable<AssetModel>> GetAllAssetsAsync(string id, string symbol, string currency, string provider);
        Task<IEnumerable<PriceModel>> RecieveMessageAsync(WSOutMessageModel message);
    }
}
