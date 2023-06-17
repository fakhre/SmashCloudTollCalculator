using SCModels.ViewModels;

namespace BLL
{
    public interface ITollManager
    {
        public VehicleEntryResponseModel VehicleCheckIn(VehicleCheckinCheckoutModel vehicleCheckinCheckoutModel);
        public FinalTollModel VehicleCheckOut(VehicleCheckinCheckoutModel vehicleCheckinCheckoutModel);

    }
}
