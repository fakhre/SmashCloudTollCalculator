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
        public bool? IsException { get; set; }
        public string? ExceptionMessage { get; set; }

    }

    public class VehicleEntryResponseModel
    {
        public bool? IsException { get; set; }
        public string? ExceptionMessage { get; set; }

    }
}
