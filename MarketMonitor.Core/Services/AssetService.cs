using AutoMapper;
using MarketMonitor.Core.Contracts.Models;
using MarketMonitor.Core.Contracts.Services;
using MarketMonitor.RI.Contracts.Clients;
using MarketMonitor.RI.Contracts.DbEntities;
using MarketMonitor.RI.Contracts.Entities;
using MarketMonitor.RI.Contracts.Repositories;
using Microsoft.Extensions.Logging;

namespace MarketMonitor.Core.Services
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepo _assetRepo;
        private readonly IPriceRepo _priceRepo;
        private readonly IFintachartsHttpClient _fintachartsHttpClient;
        private readonly IFintachartsWebSocketClient _fintachartsWebSocketClient;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public AssetService(IAssetRepo assetRepo,
            IPriceRepo priceRepo,
            IFintachartsHttpClient fintachartsHttpClient, 
            IFintachartsWebSocketClient fintachartsWebSocketClient, 
            IMapper mapper, 
            ILogger<AssetService> logger) 
        { 
            _assetRepo = assetRepo;
            _priceRepo = priceRepo;
            _fintachartsHttpClient = fintachartsHttpClient;
            _fintachartsWebSocketClient = fintachartsWebSocketClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<AssetModel>> GetAllAssetsAsync(string id, string symbol, string currency, string provider) 
        {
            Instruments instrument = await _fintachartsHttpClient.GetInstrumentsAsync();

            if (instrument == null || instrument.Assets == null)
            {
                _logger.LogError($"'_fintachartsHttpClient.GetInstrumentsAsync' did not return the data in method 'GetAllAssetsAsync'");
                throw new Exception("Sorry! Client did not return the data...");
            }

            await _assetRepo.AddAssetsAsync(instrument.Assets);

            IEnumerable<DbAsset> assets = await _assetRepo.GetAssetsAsync(id, symbol, currency, provider);

            IEnumerable<AssetModel> result = _mapper.Map<IEnumerable<DbAsset>, IEnumerable<AssetModel>>(assets);
            return result;
        }

        public async Task<IEnumerable<PriceModel>> RecieveMessageAsync(WSOutMessageModel message)
        {
            bool isSuccessed = await _fintachartsWebSocketClient.ReceiveData(_mapper.Map<WSOutMessageModel, WSOutMessage>(message));

            if (!isSuccessed) return null;

            IEnumerable<DbPrice> dbPrices = await _priceRepo.GetManyPricesAsync(price =>
                                                                                    message.InstrumentId == Guid.Empty || price.DbAsset.Id == message.InstrumentId && 
                                                                                    message.Kinds.Contains(price.Kind) &&
                                                                                    message.Type == null || price.Type == message.Type &&
                                                                                    message.Provider == null || price.Provider == message.Provider);
            IEnumerable<PriceModel> priceModels = _mapper.Map<IEnumerable<DbPrice>, IEnumerable<PriceModel>>(dbPrices);
            return priceModels;
        }
    }
}