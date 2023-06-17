using SCModels.Models;
using System.Runtime.Caching;

namespace SCModels
{
    public class TempDAL : ITempDAL
    {
        public List<Interchange> Interchanges = new List<Interchange>();
        public MemoryCache TempDB = new MemoryCache("ToolDB");
        private CacheItemPolicy _cacheItemPolicy = new CacheItemPolicy
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(60)
        };
        public TempDAL()
        {
            this.Interchanges.Add(new Interchange() { Id = 1, DistnaceFromBase = 0, IsDefault = true, Name = "Zero Point" });
            this.Interchanges.Add(new Interchange() { Id = 2, DistnaceFromBase = 5, IsDefault = false, Name = "NS" });
            this.Interchanges.Add(new Interchange() { Id = 3, DistnaceFromBase = 10, IsDefault = false, Name = "Ph4" });
            this.Interchanges.Add(new Interchange() { Id = 4, DistnaceFromBase = 17, IsDefault = false, Name = "Ferozpur" });
            this.Interchanges.Add(new Interchange() { Id = 5, DistnaceFromBase = 24, IsDefault = false, Name = "Lake City" });
            this.Interchanges.Add(new Interchange() { Id = 6, DistnaceFromBase = 29, IsDefault = false, Name = "Raiwand" });
            this.Interchanges.Add(new Interchange() { Id = 7, DistnaceFromBase = 34, IsDefault = false, Name = "Bahria" });
        }

        public bool AddVehicleRecord(string key, dynamic obj)
        {
            return TempDB.Add(key, obj, _cacheItemPolicy);
        }
        public object GetVehicleRecord(string key)
        {
            return TempDB.Get(key);
        }

        public object RemoveVehicleRecord(string key)
        {
            return TempDB.Remove(key);
        }

        public Interchange GetInterchangeByName(string name)
        {
            return this.Interchanges.Where(x => x.Name == name).FirstOrDefault();
        }
    }
}
