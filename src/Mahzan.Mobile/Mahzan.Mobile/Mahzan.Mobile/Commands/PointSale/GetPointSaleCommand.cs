using System;

namespace Mahzan.Mobile.Commands.PointSale
{
    public class GetPointSaleCommand
    {
        public Guid? PointSale { get; set; }
        
        public Guid? StoreId { get; set; }
    }
}