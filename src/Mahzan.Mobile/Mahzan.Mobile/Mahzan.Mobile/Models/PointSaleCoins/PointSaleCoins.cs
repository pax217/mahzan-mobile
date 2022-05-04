using System;

namespace Mahzan.Mobile.Models.PointSaleCoins
{
    public class PointSaleCoins
    {
        public int TenCents { get; set; }
        public int TwentyCents { get; set; }
        public int FiftyCents { get; set; }
        public int One { get; set; }
        public int Two { get; set; }
        public int Five { get; set; }
        public int Ten { get; set; }
        public Guid PointSaleStateId { get; set; }
    }
}