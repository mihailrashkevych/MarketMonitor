using AutoMapper;
using MarketMonitor.API.DTO;
using MarketMonitor.Core.Contracts.Models;

namespace MarketMonitor.API
{
    public class APIMappingProfile : Profile
    {
        public APIMappingProfile()
        {
            CreateMap<MappingModel, MappingDto>()
                .ForMember(s => s.Exchange, c => c.MapFrom(m => m.ExchangeName));
            CreateMap<MappingDto, MappingModel>()
                .ForMember(s => s.ExchangeName, c => c.MapFrom(m => m.Exchange));

            CreateMap<AssetModel, AssetDto>()
                .ForMember(s => s.Id, c => c.MapFrom(m => m.Id))
                .ForMember(s => s.Symbol, c => c.MapFrom(m => m.Symbol))
                .ForMember(s => s.Kind, c => c.MapFrom(m => m.Kind))
                .ForMember(s => s.Exchange, c => c.MapFrom(m => m.Exchange))
                .ForMember(s => s.Description, c => c.MapFrom(m => m.Description))
                .ForMember(s => s.TickSize, c => c.MapFrom(m => m.TickSize))
                .ForMember(s => s.Currency, c => c.MapFrom(m => m.Currency))
                .ForMember(s => s.BaseCurrency, c => c.MapFrom(m => m.BaseCurrency))
                .ForMember(s => s.MappingsDto, c => c.MapFrom(m => m.MappingsModel));
            CreateMap<AssetDto, AssetModel>()
                .ForMember(s => s.Id, c => c.MapFrom(m => m.Id))
                .ForMember(s => s.Symbol, c => c.MapFrom(m => m.Symbol))
                .ForMember(s => s.Kind, c => c.MapFrom(m => m.Kind))
                .ForMember(s => s.Exchange, c => c.MapFrom(m => m.Exchange))
                .ForMember(s => s.Description, c => c.MapFrom(m => m.Description))
                .ForMember(s => s.TickSize, c => c.MapFrom(m => m.TickSize))
                .ForMember(s => s.Currency, c => c.MapFrom(m => m.Currency))
                .ForMember(s => s.BaseCurrency, c => c.MapFrom(m => m.BaseCurrency))
                .ForMember(s => s.MappingsModel, c => c.MapFrom(m => m.MappingsDto));

            CreateMap<PriceDto, PriceModel>();
            CreateMap<PriceModel, PriceDto>();

            CreateMap<WSOutMessageDto, WSOutMessageModel>();
            CreateMap<WSOutMessageModel, WSOutMessageDto>();
        }
    }
}