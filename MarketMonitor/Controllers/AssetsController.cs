using AutoMapper;
using MarketMonitor.Core.Contracts.Models;
using MarketMonitor.Core.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace MarketMonitor.API.Controllers
{
    [ApiController]
    [Route("assets")]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetService _assetService;
        private readonly IMapper _mapper;
        private readonly ILogger<AssetsController> _logger;

        public AssetsController(IAssetService assetService, IMapper mapper, ILogger<AssetsController> logger)
        {
            _assetService = assetService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Route("get-assets")]
        public async Task<ActionResult<IEnumerable<AssetDto>>> GetAssets(string id = null, string symbol = null, string currency = null, string provider = null)
        {
            try
            {
                IEnumerable<AssetModel> assetsModels = await _assetService.GetAllAssetsAsync(id, symbol, currency, provider);

                IEnumerable<AssetDto> assetDto = _mapper.Map<IEnumerable<AssetModel>, IEnumerable<AssetDto>>(assetsModels);

                return Ok(assetDto);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
                return BadRequest("Somthing went wrong ;)");
            }
        }
    }
}