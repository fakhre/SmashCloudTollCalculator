using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCModels.ViewModels
{
    public class FinalTollModel
    {
        public string VehicleRegNumber { get; set; }
        public string EntryInterchangeName { get; set; }
        public string ExitInterchangeName { get; set; }
        public decimal BaseRate { get; set; }
        //public int DistanceCover { get; set; }
        public decimal SpecialDiscount { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalToll { get; set; }
        public DateTime EntryTime { get; set; }

    }
}
