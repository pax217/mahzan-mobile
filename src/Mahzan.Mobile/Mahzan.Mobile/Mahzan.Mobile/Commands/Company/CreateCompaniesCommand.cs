using System;

namespace Mahzan.Mobile.Commands.Company
{
    public class CreateCompanyCommand
    {
        public CompanyCommand Company { get; set; }
        public AdressCommand Adress { get; set; }
        public ContactCommand Contact { get; set; }
    }

    public class CompanyCommand
    {
 
        public Guid ComercialBusinessId { get; set; }

        public string Name { get; set; }

        public string RFC { get; set; } 
    }

    public class AdressCommand
    {

        public string Street { get; set; }

        public string Number { get; set; }
    
        public string InternalNumber { get; set; } 

        public string PostalCode { get; set; } 
    }

    public class ContactCommand
    {

        public string ContactName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; } 
    
        public string WebSite { get; set; } 
    }
}