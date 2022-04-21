using System;

namespace Mahzan.Mobile.Commands.Category
{
    public class CreateCategoryCommand
    {
        public string Name { get; set; }
        
        public Guid DepartmentId { get; set; }
    }
}