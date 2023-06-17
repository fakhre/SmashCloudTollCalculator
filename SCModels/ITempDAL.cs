using SCModels.Models;

namespace SCModels
{
    public interface ITempDAL
    {
        public bool AddVehicleRecord(string key, dynamic obj);
        public object RemoveVehicleRecord(string key);
        public object GetVehicleRecord(string key);
        public Interchange GetInterchangeByName(string name);
    }
}
