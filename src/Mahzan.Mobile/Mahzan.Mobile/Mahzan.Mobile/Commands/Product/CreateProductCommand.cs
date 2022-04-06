using System;
using SQLite;

namespace Mahzan.Mobile.Commands.Product
{
    public class CreateProductCommand
    {
        public CreateProductPhotoCommand Photo { get; set; }
    
        public CreateProductDetailCommand Detail { get; set; }

        public CreateProductOrganizationCommand Organization { get; set; }
    }
    public class CreateProductPhotoCommand
    {
        public string Title { get; set; }
        
        public string MIMEType { get; set; }

        public string Base64 { get; set; }

    }
    public class CreateProductDetailCommand
    {

        public string BarCode { get; set; }
        
        public string AlternativeKey { get; set; }


        public string Description { get; set; }


        public decimal? Price { get; set; }


        public decimal? Cost { get; set; }
    
        public bool FollowInventory { get; set; }
    
        public string ProductSaleUnitId { get; set; }
    
    }
    public class CreateProductOrganizationCommand
    {
        public string DepartmentId { get; set; }
    
        public string CategoryId { get; set; }
    
        public string SubCategoryId { get; set; }
    }
}