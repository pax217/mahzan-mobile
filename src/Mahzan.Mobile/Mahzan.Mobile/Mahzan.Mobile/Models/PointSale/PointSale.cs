using System;

namespace Mahzan.Mobile.Models.PointSale
{
    public class PointSale
    {
        public Guid PointSaleId { get; set; }
        
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public string State { get; set; }
        
        public Guid StoreId { get; set; }
        
        public Guid MemberId { get; set; }
    }
}