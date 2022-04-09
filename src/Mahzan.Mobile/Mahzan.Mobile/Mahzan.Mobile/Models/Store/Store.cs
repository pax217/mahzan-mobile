using System;

namespace Mahzan.Mobile.Models.Store
{
    public class Store
    {
        public Guid StoreId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Comment { get; set; }
        public bool Active { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTimeOffset DeletedAt { get; set; }
        public string DeletedBy { get; set; }
        public Guid CompanyId { get; set; }
        public Guid MemberId { get; set; }
    }
}