using System;

namespace Mahzan.Mobile.Models.SubCategory
{
    public class SubCategory
    {
        public Guid SubCategoryId { get; set; }
        public string Name { get; set; }
        
        public Guid CategoryId { get; set; }
    }
}