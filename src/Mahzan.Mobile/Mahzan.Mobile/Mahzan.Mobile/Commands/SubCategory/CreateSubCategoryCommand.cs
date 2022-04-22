using System;

namespace Mahzan.Mobile.Commands.SubCategory
{
    public class CreateSubCategoryCommand
    {
        public string Name { get; set; }
        
        public Guid CategoryId { get; set; }
    }
}