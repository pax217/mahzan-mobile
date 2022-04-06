using System;

namespace Mahzan.Mobile.Models.Product
{
    public class Product
    {
        public string Title { get; set; }
        
        public string Type { get; set; }
        public string Image { get; set; }
        public Guid ProductId { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}