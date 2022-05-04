using System;

namespace Mahzan.Mobile.Models.PointSaleState
{
    public class PointSaleState
    {
        public Guid PointSaleStateId { get; set; }
        public string State { get; set; }
        public string DateTime { get; set; }
        public decimal AmountCoins { get; set; }
        public decimal AmountBills { get; set; }
        public Guid PointSaleId { get; set; }

        public PointSaleCoins.PointSaleCoins Coins { get; set; }
        public PointSaleBills.PointSaleBills Bills { get; set; }
        
    }
}