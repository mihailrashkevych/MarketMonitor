using AutoMapper;
using MarketMonitor.API.DTO;
using MarketMonitor.Core.Contracts.Models;
using MarketMonitor.Core.Contracts.Services;
using MarketMonitor.RI.Contracts.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MarketMonitor.API.Controllers
{
    [ApiController]
    [Route("price")]
    public class PriceController : ControllerBase
    {
        private readonly IAssetService _assetService;
        private readonly IMapper _mapper;
        private readonly ILogger<AssetsController> _logger;

        public PriceController(IAssetService assetService, IMapper mapper, ILogger<AssetsController> logger)
        {
            _assetService = assetService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPut]
        [Route("get-current-price")]
        public async Task<ActionResult<IEnumerable<PriceDto>>> GetCurrentPrice( IEnumerable<string> kinds,
                                                                                string instrumentId = "ad9e5345-4c3b-41fc-9437-1d253f62db52",
                                                                                string type = "l1-subscription",
                                                                                string id = "1",
                                                                                string provider = "simulation",
                                                                                bool subscribe =true)
        {
            try
            {
                if (kinds.FirstOrDefault() == "string") kinds = ["ask", "bid", "last"];

                var message = new WSOutMessageDto();
                message.Type = type;
                message.Id = id;
                message.InstrumentId = Guid.Parse(instrumentId);
                message.Provider = provider;
                message.Subscribe = subscribe;
                message.Kinds = kinds;

                IEnumerable<PriceModel> priceModels = await _assetService.RecieveMessageAsync(_mapper.Map<WSOutMessageDto, WSOutMessageModel>(message));

                IEnumerable<PriceDto> priceDtos = _mapper.Map<IEnumerable<PriceModel>, IEnumerable<PriceDto>>(priceModels);

                if (priceDtos == null) return BadRequest("Data did not received ;)");
                return Ok(priceDtos);
            }
            catch (Exception)
            {
                return BadRequest("Somthing went wrong ;)_");
            }

        }              
    }
}
