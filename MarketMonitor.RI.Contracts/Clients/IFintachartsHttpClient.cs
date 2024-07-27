using MarketMonitor.RI.Contracts.Entities;
namespace MarketMonitor.RI.Contracts.Clients
{
    public interface IFintachartsHttpClient
    {
        Task<string?> GetTokenAsync();
        Task<Instruments?> GetInstrumentsAsync(string? provider = null, string? currency = null, string? symbol = null, string? id = null);
        Task<Instruments?> GetInstrumentsAsync();
    }
}
