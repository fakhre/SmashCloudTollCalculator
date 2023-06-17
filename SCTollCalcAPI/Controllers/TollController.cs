using BLL;
using Microsoft.AspNetCore.Mvc;
using SCModels.ViewModels;

namespace SCTollCalcAPI.Controllers
{
    public class TollController : BaseController
    {
        private ITollManager _tollManager;
        public TollController(ITollManager tollManager)
        {
            _tollManager = tollManager;
        }

        [HttpPost("VehicleEntry")]
        public IActionResult CreateOffer(VehicleCheckinCheckoutModel vehicleCheckinRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(vehicleCheckinRequest);
                }
                var response = _tollManager.VehicleCheckIn(vehicleCheckinRequest);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("VehicleExit")]
        public IActionResult VehicleExit(VehicleCheckinCheckoutModel vehicleCheckoutRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(vehicleCheckoutRequest);
                }
                var response = _tollManager.VehicleCheckOut(vehicleCheckoutRequest);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }


}
