using MarketMonitor.RI.Contracts.DbEntities;

namespace MarketMonitor.DataStore.SQL.Tools
{
    internal class DbMappingHashSetComparer : IEqualityComparer<DbMapping>
    {
        public DbMappingHashSetComparer(){ }
        public bool Equals(DbMapping dbMapping1, DbMapping dbMapping2)
        {
            return dbMapping1.DbAsset.Id == dbMapping2.DbAsset.Id 
                && dbMapping2.ExchangeName == dbMapping2.ExchangeName 
                && dbMapping2.DefaultOrderSize == dbMapping2.DefaultOrderSize
                && dbMapping2.ProviderName == dbMapping2.ProviderName;
        }
        public int GetHashCode(DbMapping t)
        {
            string code = t.DbAsset.Id + "," + t.ExchangeName + "," + t.DefaultOrderSize + "," + t.ProviderName;
            return code.GetHashCode();
        }
    }
}
