using System;

namespace Mahzan.Mobile.Models.Category
{
    public class Category
    {
        public Guid CategoryId { get; set; }

        public string Name { get; set; }
        
        public Guid DepartmentId { get; set; }
    }
}