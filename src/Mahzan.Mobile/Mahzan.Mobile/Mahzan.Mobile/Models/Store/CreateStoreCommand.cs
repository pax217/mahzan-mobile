namespace Mahzan.Mobile.Models.Store
{
    public class CreateStoreCommand
    {
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public string Phone { get; set; }
        
        public string Comment { get; set; }
        
        public string CompanyId { get; set; }
    }
}