using System;

namespace Mahzan.Mobile.Models.PointSaleState
{
    public class PointSaleState
    {

        public Guid PointSaleStateId { get; set; }
        public string State { get; set; }
        
        public DateTimeOffset DateTime { get; set; }
    
        public Guid UserId { get; set; }
        public decimal AmountBills { get; set; }
        public decimal AmountCoins { get; set; }
        
        public Guid PointSaleId { get; set; }
        
    }
}