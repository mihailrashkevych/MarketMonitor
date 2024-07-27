using MarketMonitor.RI.Contracts.DbEntities;

namespace MarketMonitor.DataStore.SQL.Tools
{
    public class DbAssetHashSetComparer : IEqualityComparer<DbAsset>
    {
        public DbAssetHashSetComparer() { }

        public bool Equals(DbAsset dbAsset1, DbAsset dbAsset2)
        {
            return dbAsset1.Id == dbAsset2.Id && dbAsset1.Currency == dbAsset2.Currency && dbAsset1.Exchange == dbAsset2.Exchange;
        }

        public int GetHashCode(DbAsset t)
        {
            string code = t.Id + "," + t.Currency + "," + t.Exchange;
            return code.GetHashCode();
        }
    }
}
