using MarketMonitor.Core.Contracts.Models;

namespace MarketMonitor.Core.Contracts.Models
{
    public record InstrumentsModel
    {
        public PagingModel? Paging { get; set; }
        public IEnumerable<AssetModel>? Assets  { get; set; }
    }
}
