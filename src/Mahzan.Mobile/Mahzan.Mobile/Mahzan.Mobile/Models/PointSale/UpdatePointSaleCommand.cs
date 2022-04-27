using System;

namespace Mahzan.Mobile.Models.PointSale
{
    public class UpdatePointSaleCommand
    {
        public Guid PointSaleId { get; set; }
        
        public string Name   { get; set; }
        
        public string Code   { get; set; }
        
        public Guid StoreId { get; set; }
    }
}