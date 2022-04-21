using System;
using Mahzan.Mobile.Models.Menu.Products;

namespace Mahzan.Mobile.Commands.Department
{
    public class UpdateDepartmentCommand
    {
        public Guid DepartmentId { get; set; }
        
        public string Name { get; set; }
        
        public Guid StoreId { get; set; }
    }
}