using System;

namespace Mahzan.Mobile.Models.PointSaleBills
{
    public class PointSaleBills
    {
        public int Twenty { get; set; }
        public int Fifty { get; set; }
        public int Hundred { get; set; }
        public int TwoHundred { get; set; }
        public int FiveHundred { get; set; }
        public int OneThousand { get; set; }
        public Guid PointSaleStateId { get; set; }
    }
}