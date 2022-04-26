using System;

namespace Mahzan.Mobile.Models.UnitSale
{
    public class UnitSale
    {
        public Guid UnitSaleId { get; set; }
        
        public string Abbreviation { get; set; }
        
        public string Description { get; set; }
        
        public Guid Member { get; set; }
    }
}