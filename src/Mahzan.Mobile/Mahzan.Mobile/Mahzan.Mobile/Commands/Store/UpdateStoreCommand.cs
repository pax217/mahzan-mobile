using System;

namespace Mahzan.Mobile.Commands.Store
{
    public class UpdateStoreCommand
    {
        public Guid StoreId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Comment { get; set; }
        
        public string CompanyId { get; set; }
    }
}