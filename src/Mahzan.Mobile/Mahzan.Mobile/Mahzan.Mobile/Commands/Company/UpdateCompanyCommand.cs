using System;

namespace Mahzan.Mobile.Commands.Company
{
    public class UpdateCompanyCommand
    {
        public UpdateCompany Company { get; set; }
        public UpdateAdress Adress { get; set; }
        public UpdateContact Contact { get; set; }
    }

    public class UpdateCompany
    {
        public Guid CompanyId { get; set; }
        public Guid? ComercialBusinessId { get; set; }
        public string Name { get; set; }
        public string RFC { get; set; } 
    }

    public class UpdateAdress
    {
        public string Street { get; set; }

        public string Number { get; set; }
    
        public string InternalNumber { get; set; }
    
        public string PostalCode { get; set; } 
    }

    public class UpdateContact
    {
        public string ContactName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string WebSite { get; set; } 
    }
}