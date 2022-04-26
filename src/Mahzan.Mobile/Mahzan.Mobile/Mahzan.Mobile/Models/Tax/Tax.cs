using System;

namespace Mahzan.Mobile.Models.Tax
{
    public class Tax
    {
        public Guid TaxId { get; set; }
        
        public string Name { get; set; }
        
        public decimal Percentage { get; set; }
        
        public bool Active { get; set; }
        
        public Guid MemberId { get; set; }
    }
}