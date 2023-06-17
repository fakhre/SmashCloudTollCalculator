using SCModels;
using SCModels.Models;
using SCModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class TollManager : ITollManager
    {
        private ITempDAL _tempDAL;
        public TollManager(ITempDAL tempDAL)
        {
            _tempDAL = tempDAL;
        }

        public bool VehicleCheckIn(VehicleCheckinCheckoutModel model)
        {
            return _tempDAL.AddVehicleRecord(model.VehicleRegNumber, model);
        }

        public FinalTollModel VehicleCheckOut(VehicleCheckinCheckoutModel exitModel)
        {
            VehicleCheckinCheckoutModel entryModel = (VehicleCheckinCheckoutModel)_tempDAL.GetVehicleRecord(exitModel.VehicleRegNumber);
            decimal totalToll, finalToll, distanceCost;
            decimal specialDiscount = 0;
            decimal baseToll = 20;
            decimal perKMTollRate = 0.2m;

            if (exitModel.EntryTime.DayOfWeek == DayOfWeek.Saturday || exitModel.EntryTime.DayOfWeek == DayOfWeek.Sunday)
                perKMTollRate = 0.3m;


            Interchange entryInterchange = _tempDAL.GetInterchangeByName(entryModel.InterchangeName);
            Interchange exiteInterchange = _tempDAL.GetInterchangeByName(exitModel.InterchangeName);


            int distanceTraveled = exiteInterchange.DistnaceFromBase - entryInterchange.DistnaceFromBase;
            if (distanceTraveled < 0)
                distanceTraveled *= -1;

            finalToll = totalToll = baseToll + (perKMTollRate * distanceTraveled);

            bool isEven = CheckIsEven(exitModel.VehicleRegNumber);
            if (isEven)
            {
                if (entryModel.EntryTime.DayOfWeek == DayOfWeek.Monday || entryModel.EntryTime.DayOfWeek == DayOfWeek.Wednesday)
                {
                    decimal splDiscount = (finalToll * 10) / 100;
                    finalToll = finalToll - splDiscount;
                }
            }
            else
            {
                if (entryModel.EntryTime.DayOfWeek == DayOfWeek.Tuesday || entryModel.EntryTime.DayOfWeek == DayOfWeek.Thursday)
                {
                    decimal splDiscount = (finalToll * 10) / 100;
                    finalToll = finalToll - splDiscount;
                }
            }

            //holiday 50% discount on toll  
            int currentDay = DateTime.Now.Day;
            int currentMonth = DateTime.Now.Month;
            if ((currentDay == 23 && currentMonth == 3) ||
                (currentDay == 14 && currentMonth == 8) ||
                (currentDay == 25 && currentMonth == 12))
            {
                specialDiscount = finalToll / 2;
                finalToll = finalToll / 2;
            }
            FinalTollModel finalTollModel = new FinalTollModel()
            {
                BaseRate = baseToll,
                Discount = totalToll - finalToll,
                TotalToll = finalToll,
                SpecialDiscount = specialDiscount,
                EntryTime = entryModel.EntryTime,
                EntryInterchangeName = entryModel.InterchangeName,
                ExitInterchangeName = exiteInterchange.Name,
                VehicleRegNumber = entryModel.VehicleRegNumber
            };
            return finalTollModel;
        }

        private bool CheckIsEven(string vehicleRegNumber)
        {
            int number = Convert.ToInt32(vehicleRegNumber.Split("-")[1]);
            if (number % 2 == 0)
                return true;
            else return false;
        }
    }
}
