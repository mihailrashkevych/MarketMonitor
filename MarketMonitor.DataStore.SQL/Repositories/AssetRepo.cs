using AutoMapper;
using MarketMonitor.DataStore.SQL.Tools;
using MarketMonitor.RI.Contracts.DbEntities;
using MarketMonitor.RI.Contracts.Entities;
using MarketMonitor.RI.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MarketMonitor.DataStore.SQL.Repositories
{
    public class AssetRepo : IAssetRepo, IDisposable
    {
        private readonly SQLDBContext _sQLDBContext;
        private readonly IMapper _mapper;
        public AssetRepo(SQLDBContext sQLiteDBContext, IMapper mapper) {
            _sQLDBContext = sQLiteDBContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DbAsset>> GetAssetsAsync(string id, string symbol, string currency, string provider)
        {
            IEnumerable<DbAsset> assets = await _sQLDBContext.Assets.Include(e => e.DbMappings)
                .Where(a => (id ==null || a.Id == Guid.Parse(id)) &&
                    (symbol == null || a.Symbol == symbol) &&
                    (currency == null || a.Currency == currency) &&
                    a.DbMappings.Any(m => provider == null || m.ProviderName == provider)).ToListAsync();
            return assets;
        }

        public IEnumerable<DbAsset> GetAssetsAsync(Func<DbAsset, bool> predicate)
        {
            IEnumerable<DbAsset> assets = _sQLDBContext.Assets.Include(e => e.DbMappings).Where(predicate).ToList();
            return assets;
        }

        public async Task AddAssetsAsync(IEnumerable<Asset> assets)
        {
            HashSet<DbAsset> dbAssetsHS = new HashSet<DbAsset>(new DbAssetHashSetComparer());
            dbAssetsHS.UnionWith(_sQLDBContext.Assets.ToHashSet());

            HashSet<DbMapping> dbMappingHS = new HashSet<DbMapping>(new DbMappingHashSetComparer());
            dbMappingHS.UnionWith(_sQLDBContext.Mappings.ToHashSet());


            foreach (var asset in assets)
            {
                DbAsset dbAsset = _mapper.Map<Asset, DbAsset>(asset);
                bool isAssetExist = dbAssetsHS.Contains<DbAsset>(dbAsset);
                if (!isAssetExist)
                {
                     await _sQLDBContext.Assets.AddAsync(dbAsset);
                }

                foreach (var exchange in asset.Mappings)
                {
                    DbMapping dbMapping = new DbMapping();
                    dbMapping.ExchangeName = exchange.Value.Exchange;
                    dbMapping.ProviderName = exchange.Key;
                    dbMapping.Symbol = exchange.Value.Symbol;
                    dbMapping.DefaultOrderSize = exchange.Value.DefaultOrderSize;
                    dbMapping.DbAsset = dbAsset;

                    bool isMappingExist = dbMappingHS.Contains<DbMapping>(dbMapping);
                    if (!isMappingExist)
                    {
                        await _sQLDBContext.Mappings.AddAsync(dbMapping);
                    }
                }
            }
                await _sQLDBContext.SaveChangesAsync();
        }

        public void Save()
        {
            _sQLDBContext.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _sQLDBContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
