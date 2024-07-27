using MarketMonitor.RI.Contracts.Clients;
using MarketMonitor.RI.Contracts.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text;
using MarketMonitor.RI.Contracts.DbEntities;
using MarketMonitor.RI.Contracts.Repositories;

namespace MarketMonitor.DataProvider.Fintacharts
{
    public class FintachartsWSClient : IFintachartsWebSocketClient
    {
        private readonly IConfiguration _configuration;
        private readonly IFintachartsHttpClient _httpClient;
        private readonly IPriceRepo _priceRepo;
        private readonly IAssetRepo _assetRepo;
        private readonly ILogger _logger;
        public FintachartsWSClient(IConfiguration configuration, 
            IFintachartsHttpClient fintachartsHttpClient, 
            IAssetRepo assetRepo,
            IPriceRepo priceRepo,
            ILogger<FintachartsWSClient> logger)
        {
            _configuration = configuration;
            _httpClient = fintachartsHttpClient;
            _assetRepo = assetRepo;
            _priceRepo = priceRepo;
            _logger = logger;
        }

        public async Task<bool> ReceiveData(WSOutMessage message)
        {
            string token = await _httpClient.GetTokenAsync();
            bool isDataReceived = false;
            try
            {
                using (var ws = new ClientWebSocket())
                {
                    Uri uri = new Uri($"wss://platform.fintacharts.com/api/streaming/ws/v1/realtime?token={token}");
                    await ws.ConnectAsync(uri, CancellationToken.None);

                    byte[] receiveBuffer = new byte[1024];
                    WebSocketReceiveResult receiveResult = await ws.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
                    string receivedMessage = Encoding.UTF8.GetString(receiveBuffer, 0, receiveResult.Count);

                    byte[] buffer = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
                    await ws.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);

                    List<string> receivedKinds = new List<string>();

                    while (ws.State == WebSocketState.Open && !isDataReceived)
                    {
                        await ReceiveAndStoreData(ws, message, receivedKinds);


                        IEnumerable<string> list = message.Kinds.Except(receivedKinds);
                        isDataReceived = !list.Any();
                    }
                }

                return isDataReceived;
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get the latest data: " + e.Message);
                return false;
            }
        }

        private async Task ReceiveAndStoreData(ClientWebSocket ws, WSOutMessage wSOutMessage, List<string> receivedKinds)
        {
            byte[] receiveBuffer = new byte[1024];
            var timeOut = new CancellationTokenSource(5000).Token;
            WebSocketReceiveResult receiveResult = await ws.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), timeOut);
            string receivedMessage = Encoding.UTF8.GetString(receiveBuffer, 0, receiveResult.Count);

            //Start Mapping
            //check the message of which type received and mapped
            WSInMessage wSInMessage = JsonSerializer.Deserialize<WSInMessage>(receivedMessage);

            if (receivedMessage == "{\"type\":\"response\",\"requestId\":\"1\"}") return;
            if (wSInMessage == null)
            {
                ws.CloseAsync(WebSocketCloseStatus.InvalidMessageType, "", CancellationToken.None);
                return;
            }

            foreach (string kind in wSOutMessage.Kinds) 
            {
                if (receivedMessage.Contains(kind))
                {
                    wSInMessage.Kind = kind;

                    //add kinds to check do we get all kinds requested by user
                    if (!receivedKinds.Contains(kind)) receivedKinds.Add(kind);
                }
            }



            if (wSInMessage.InstrumentId != Guid.Empty) 
            {
                IEnumerable<DbAsset> dbAssets = await _assetRepo.GetAssetsAsync();
                Dictionary<Guid,DbAsset> AssetsDictionary = new Dictionary<Guid, DbAsset>();

                foreach (var item in dbAssets)
                {
                    AssetsDictionary.Add(item.Id,item);
                }

                DbAsset dbAsset = new DbAsset();
                if (AssetsDictionary.TryGetValue(wSInMessage.InstrumentId, out dbAsset))
                {
                    DbPrice dbPrice = new DbPrice();
                    dbPrice.Provider = wSInMessage.Provider;
                    dbPrice.Type = wSInMessage.Type;
                    dbPrice.DbAsset = dbAsset;

                    PriceInfo[] priceInfos = [wSInMessage.BidInfo, wSInMessage.AskInfo, wSInMessage.LastInfo];

                    bool isPicePresent = false;
                    for (int i = 0; i < priceInfos.Length; i++)
                    {
                        if (priceInfos[i] != null && priceInfos[i] != null)
                        {
                            dbPrice.Time = priceInfos[i].Time;
                            dbPrice.Price = priceInfos[i].Price;
                            dbPrice.Volume = priceInfos[i].Volume;
                            dbPrice.Change = priceInfos[i].Change;
                            dbPrice.ChangePercents = priceInfos[i].ChangePercents;
                            dbPrice.Kind = wSInMessage.Kind;

                            isPicePresent = true;
                        }
                    }
                    //End Mapping
                    if (!isPicePresent) _logger.LogInformation(nameof(ReceiveAndStoreData) + " :PriceInfo not return from data source");

                    //DB add/update
                    if (!await _priceRepo.TryUpdatePriceWith(x => x.DbAsset == dbAsset && x.Kind == dbPrice.Kind && x.Provider == dbPrice.Provider, dbPrice))
                    {
                        await _priceRepo.AddPrice(dbPrice);
                    }
                }
                else
                {
                    _logger.LogInformation(nameof(ReceiveAndStoreData) +" :Asset/Instrument not present in DB");
                }
            }
            else
            {
                _logger.LogInformation(nameof(ReceiveAndStoreData) + " :Input message not valid");
            }
        }
    }
}