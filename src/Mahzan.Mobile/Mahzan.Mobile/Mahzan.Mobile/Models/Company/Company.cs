using System;

namespace Mahzan.Mobile.Models.Company
{
    public class Company
    {
        public Guid CompanyId { get; set; }
    
        public string CompanyName { get; set; }

        public string RFC { get; set; }
    
        public string Street { get; set; }
    
        public string Number { get; set; }
    
        public string InternalNumber { get; set; }
    
        public string PostalCode { get; set; }
    
        public string ContactName { get; set; }
    
        public string Email { get; set; }
    
        public string Phone { get; set; }
    
        public string WebSite { get; set; }
    
        public Guid CommercialBusinessId { get; set; }
    
        public Guid MemberId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    
        public string CreatedBy { get; set; }
    
        public DateTimeOffset UpdatedAt { get; set; }
    
        public string UpdatedBy { get; set; }
        
        public string Logo { get; set; }
    }
}