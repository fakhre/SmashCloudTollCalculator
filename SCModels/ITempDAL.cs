using SCModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCModels
{
    public interface ITempDAL
    {
        public bool AddVehicleRecord(string key, dynamic obj);
        public object GetVehicleRecord(string key);
        public Interchange GetInterchangeByName(string name);
    }
}
