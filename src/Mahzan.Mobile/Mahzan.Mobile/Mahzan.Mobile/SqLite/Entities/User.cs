using System;
using SQLite;

namespace Mahzan.Mobile.SqLite.Entities
{
    public class User
    {
        [PrimaryKey]
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        
        // Store
        public Guid StoreId { get; set; }
        public string StoreName { get; set; }
        
        // Point Sale
        public Guid PointSaleId { get; set; }
        public string PointSaleName { get; set; }

    }
}