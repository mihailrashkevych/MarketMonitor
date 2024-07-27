using MarketMonitor.RI.Contracts.DbEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MarketMonitor.RI.Contracts.Repositories
{
    public interface IPriceRepo
    {
        Task AddPrice(DbPrice dbPrice);
        Task<IEnumerable<DbPrice>> GetManyPricesAsync(Expression<Func<DbPrice, bool>> condition);
        Task<DbPrice> GetPriceAsync(Expression<Func<DbPrice, bool>> where);
        Task<bool> TryUpdatePriceWith(Expression<Func<DbPrice, bool>> condition, DbPrice price);
    }
}
