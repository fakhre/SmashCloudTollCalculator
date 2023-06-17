using SCModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface ITollManager
    {
        public bool VehicleCheckIn(VehicleCheckinCheckoutModel vehicleCheckinCheckoutModel);
        public FinalTollModel VehicleCheckOut(VehicleCheckinCheckoutModel vehicleCheckinCheckoutModel);

    }
}
