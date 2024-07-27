using AutoMapper;
using MarketMonitor.RI.Contracts.DbEntities;
using MarketMonitor.RI.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MarketMonitor.DataStore.SQL.Repositories
{
    public class PriceRepo: IPriceRepo, IDisposable
    {
        private readonly SQLDBContext _sQLDBContext;
        private readonly IMapper _mapper;
        public PriceRepo(SQLDBContext sQLiteDBContext, IMapper mapper)
        {
            _sQLDBContext = sQLiteDBContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DbPrice>> GetManyPricesAsync(Expression<Func<DbPrice, bool>> condition)
        {
            return await _sQLDBContext.Prices.Include(a=>a.DbAsset).Where(condition).ToListAsync();
        }

        public async Task<DbPrice> GetPriceAsync(Expression<Func<DbPrice, bool>> condition)
        {
            return await _sQLDBContext.Prices.Include(a => a.DbAsset).Where(condition).FirstOrDefaultAsync();
        }

        public async Task AddPrice(DbPrice dbPrice)
        {
            await _sQLDBContext.Prices.AddAsync(dbPrice);
            await SaveAsync();
        }

        public async Task<bool> TryUpdatePriceWith(Expression<Func<DbPrice, bool>> condition, DbPrice price)
        {
            DbPrice dbPrice = await GetPriceAsync(condition);

            if (dbPrice != null)
            {
                dbPrice.Provider = price.Provider;
                dbPrice.Change = price.Change;
                dbPrice.ChangePercents = price.ChangePercents;
                dbPrice.Time = price.Time;
                dbPrice.Price = price.Price;
                dbPrice.Volume = price.Volume;
                dbPrice.Type = price.Type;
                dbPrice.Kind = price.Kind;
                dbPrice.DbAsset = price.DbAsset;

                await SaveAsync();
                return true;
            }
            else { return false; }
        }

        private async Task SaveAsync()
        {
            await _sQLDBContext.SaveChangesAsync();
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