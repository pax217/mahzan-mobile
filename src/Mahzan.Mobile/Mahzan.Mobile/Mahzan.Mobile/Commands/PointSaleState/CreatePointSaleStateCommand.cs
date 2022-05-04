using System;

namespace Mahzan.Mobile.Commands.PointSaleState
{
    public class CreatePointSaleStateCommand
    {
        public Guid PointSaleId { get; set; }
        public Bills Bills { get; set; }
        public Coins Coins { get; set; }     
    }

    public class Bills
    {
        public int Twenty { get; set; }
        public int Fifty { get; set; }
        public int Hundred { get; set; }
        public int TwoHundred { get; set; }
        public int FiveHundred { get; set; }
        public int OneThousand { get; set; }
    }

    public class Coins
    {
        public int TenCents { get; set; }
        public int TwentyCents { get; set; }
        public int FiftyCents { get; set; }
        public int One { get; set; }
        public int Two { get; set; }
        public int Five { get; set; }
        public int Ten { get; set; }
    }
}