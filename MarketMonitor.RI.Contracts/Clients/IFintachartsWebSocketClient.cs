using MarketMonitor.RI.Contracts.Entities;

namespace MarketMonitor.RI.Contracts.Clients
{
    public interface IFintachartsWebSocketClient
    {
        Task<bool> ReceiveData(WSOutMessage message);
    }
}
