using System;

namespace Mahzan.Mobile.Commands.PointSale
{
    public class CreatePointSaleCommand
    {
        public string Name   { get; set; }
        
        public string Code   { get; set; }
        
        public Guid StoreId { get; set; }
    }
}