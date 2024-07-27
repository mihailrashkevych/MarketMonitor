using AutoMapper;
using MarketMonitor.Core.Contracts.Models;
using MarketMonitor.RI.Contracts.DbEntities;
using MarketMonitor.RI.Contracts.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketMonitor.API
{
    public class CoreMappingProfile : Profile
    {
        public CoreMappingProfile()
        {
            CreateMap<AssetModel, Asset>();
            CreateMap<Asset, AssetModel>();

            CreateMap<InstrumentsModel, Instruments>();
            CreateMap<Instruments, InstrumentsModel>();

            CreateMap<PagingModel, Paging>();
            CreateMap<Paging, PagingModel>();

            CreateMap<MappingModel, DbMapping>()
                .ForMember(mm => mm.Id, m => m.MapFrom(dm => dm.Id))
                .ForMember(mm => mm.ProviderName, m => m.MapFrom(dm => dm.ProviderName))
                .ForMember(mm => mm.ExchangeName, m => m.MapFrom(dm => dm.ExchangeName))
                .ForMember(mm => mm.Symbol, m => m.MapFrom(dm => dm.Symbol))
                .ForMember(mm => mm.DefaultOrderSize, m => m.MapFrom(dm => dm.DefaultOrderSize));

            CreateMap<DbMapping, MappingModel>()
                .ForMember(mm => mm.Id, m=> m.MapFrom(dm => dm.Id))
                .ForMember(mm => mm.ProviderName, m => m.MapFrom(dm => dm.ProviderName))
                .ForMember(mm => mm.ExchangeName, m => m.MapFrom(dm => dm.ExchangeName))
                .ForMember(mm => mm.Symbol, m => m.MapFrom(dm => dm.Symbol))
                .ForMember(mm => mm.DefaultOrderSize, m => m.MapFrom(dm => dm.DefaultOrderSize));

            CreateMap<Asset, DbAsset>();
            CreateMap<DbAsset, Asset>();

            CreateMap<AssetModel, DbAsset>()
                .ForMember(s => s.Id, c => c.MapFrom(m => m.Id))
                .ForMember(s => s.Symbol, c => c.MapFrom(m => m.Symbol))
                .ForMember(s => s.Kind, c => c.MapFrom(m => m.Kind))
                .ForMember(s => s.Exchange, c => c.MapFrom(m => m.Exchange))
                .ForMember(s => s.Description, c => c.MapFrom(m => m.Description))
                .ForMember(s => s.TickSize, c => c.MapFrom(m => m.TickSize))
                .ForMember(s => s.Currency, c => c.MapFrom(m => m.Currency))
                .ForMember(s => s.BaseCurrency, c => c.MapFrom(m => m.BaseCurrency))
                .ForMember(s => s.DbMappings, c => c.MapFrom(m => m.MappingsModel));

            CreateMap<DbAsset, AssetModel>()
                .ForMember(s => s.Id, c => c.MapFrom(m => m.Id))
                .ForMember(s => s.Symbol, c => c.MapFrom(m => m.Symbol))
                .ForMember(s => s.Kind, c => c.MapFrom(m => m.Kind))
                .ForMember(s => s.Exchange, c => c.MapFrom(m => m.Exchange))
                .ForMember(s => s.Description, c => c.MapFrom(m => m.Description))
                .ForMember(s => s.TickSize, c => c.MapFrom(m => m.TickSize))
                .ForMember(s => s.Currency, c => c.MapFrom(m => m.Currency))
                .ForMember(s => s.BaseCurrency, c => c.MapFrom(m => m.BaseCurrency))
                .ForMember(s => s.MappingsModel, c => c.MapFrom(m => m.DbMappings));

            CreateMap<DbPrice, PriceModel>().ForMember(t => t.InstrumentId, s => s.MapFrom( pr => pr.DbAsset.Id));

            CreateMap<WSOutMessage, WSOutMessageModel>();
            CreateMap<WSOutMessageModel, WSOutMessage>();
        }
    }
}
