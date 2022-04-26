using System;

namespace Mahzan.Mobile.Commands.UnitSale
{
    public class UpdateUnitSaleCommand
    {
        public Guid UnitSaleId { get; set; }
        
        public string Abbreviation { get; set; }
        
        public string Description { get; set; }
    }
}