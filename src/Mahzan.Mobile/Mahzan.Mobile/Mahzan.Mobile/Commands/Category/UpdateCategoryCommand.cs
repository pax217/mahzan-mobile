using System;

namespace Mahzan.Mobile.Commands.Category
{
    public class UpdateCategoryCommand
    {
        public Guid CategoryId { get; set; }
        
        public string Name { get; set; }
        
        public Guid DepartmentId { get; set; }
    }
}