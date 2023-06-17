using SCModels;
using SCModels.Models;
using SCModels.ViewModels;

namespace BLL
{
    public class TollManager : ITollManager
    {
        private ITempDAL _tempDAL;
        public TollManager(ITempDAL tempDAL)
        {
            _tempDAL = tempDAL;
        }

        public VehicleEntryResponseModel VehicleCheckIn(VehicleCheckinCheckoutModel model)
        {
            if (!model.VehicleRegNumber.Contains("-"))
                return new VehicleEntryResponseModel()
                {
                    IsException = true,
                    ExceptionMessage = Lookups.InvalidNumberPlateError
                };

            Interchange entryInterchange = _tempDAL.GetInterchangeByName(model.InterchangeName);

            if (entryInterchange == null)
                return new VehicleEntryResponseModel()
                {
                    IsException = true,
                    ExceptionMessage = Lookups.NoExchangeFoundError
                };

            var recrAdded = _tempDAL.AddVehicleRecord(model.VehicleRegNumber, model);
            return new VehicleEntryResponseModel()
            {
                IsException = !recrAdded
            };
        }

        public FinalTollModel VehicleCheckOut(VehicleCheckinCheckoutModel exitModel)
        {
            VehicleCheckinCheckoutModel entryModel = (VehicleCheckinCheckoutModel)_tempDAL.GetVehicleRecord(exitModel.VehicleRegNumber);
            if (entryModel == null)
                return new FinalTollModel()
                {
                    IsException = true,
                    ExceptionMessage = Lookups.NoVehicleRecordFoundError
                };

            decimal totalToll, finalToll, distanceCost;
            decimal specialDiscount = 0;
            decimal baseToll = 20;
            decimal perKMTollRate = 0.2m;

            if (exitModel.TimeStump.DayOfWeek == DayOfWeek.Saturday || exitModel.TimeStump.DayOfWeek == DayOfWeek.Sunday)
                perKMTollRate = 0.3m;


            Interchange entryInterchange = _tempDAL.GetInterchangeByName(entryModel.InterchangeName);
            Interchange exiteInterchange = _tempDAL.GetInterchangeByName(exitModel.InterchangeName);

            if (entryInterchange == null || exiteInterchange == null)
                return new FinalTollModel()
                {
                    IsException = true,
                    ExceptionMessage = Lookups.NoExchangeFoundError
                };

            int distanceTraveled = exiteInterchange.DistnaceFromBase - entryInterchange.DistnaceFromBase;
            if (distanceTraveled < 0)
                distanceTraveled *= -1;

            finalToll = totalToll = baseToll + (perKMTollRate * distanceTraveled);

            if (!exitModel.VehicleRegNumber.Contains("-"))
                return new FinalTollModel()
                {
                    IsException = true,
                    ExceptionMessage = Lookups.InvalidNumberPlateError
                };

            bool isEven = CheckIsEven(exitModel.VehicleRegNumber);
            if (isEven)
            {
                if (entryModel.TimeStump.DayOfWeek == DayOfWeek.Monday || entryModel.TimeStump.DayOfWeek == DayOfWeek.Wednesday)
                {
                    decimal splDiscount = (finalToll * 10) / 100;
                    finalToll = finalToll - splDiscount;
                }
            }
            else
            {
                if (entryModel.TimeStump.DayOfWeek == DayOfWeek.Tuesday || entryModel.TimeStump.DayOfWeek == DayOfWeek.Thursday)
                {
                    decimal splDiscount = (finalToll * 10) / 100;
                    finalToll = finalToll - splDiscount;
                }
            }

            //holiday 50% discount on toll  
            int currentDay = entryModel.TimeStump.Day;      
            int currentMonth = entryModel.TimeStump.Month;  
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
                EntryTime = entryModel.TimeStump,
                EntryInterchangeName = entryModel.InterchangeName,
                ExitInterchangeName = exiteInterchange.Name,
                VehicleRegNumber = entryModel.VehicleRegNumber,
                IsException = false
            };
            //romve entry.. 
            _tempDAL.RemoveVehicleRecord(entryModel.VehicleRegNumber); //TODO::preserve for log... 
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
