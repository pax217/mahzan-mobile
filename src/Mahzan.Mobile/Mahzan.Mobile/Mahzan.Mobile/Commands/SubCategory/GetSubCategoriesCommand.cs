using System;

namespace Mahzan.Mobile.Commands.SubCategory
{
    public class GetSubCategoriesCommand
    {
        public Guid? SubCategoryId { get; set; }
        public Guid? CategoryId { get; set; }
    }
}