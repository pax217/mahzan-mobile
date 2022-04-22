using System;

namespace Mahzan.Mobile.Commands.SubCategory
{
    public class UpdateSubCategoryCommand
    {
        public Guid SubCategoryId { get; set; }
    
        public string Name { get; set; }
        
        public Guid CategoryId { get; set; }
    }
}