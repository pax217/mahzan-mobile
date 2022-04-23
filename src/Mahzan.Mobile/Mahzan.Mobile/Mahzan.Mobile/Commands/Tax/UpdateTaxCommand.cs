using System;

namespace Mahzan.Mobile.Commands.Tax
{
    public class UpdateTaxCommand
    {
        public Guid TaxId { get; set; }
        
        public string Name { get; set; }
        
        public decimal? Percentage { get; set; }
    }
}