using System.Collections.Generic;

namespace WebApI.Models
{
    public class ProductsDTO
    {
        public long ProductId { get; set; }
        public int ProdCatId { get; set; }
        public string ProdName { get; set; }
        public string ProdDescription { get; set; }

        //public List<ProductAttribute> ProdAttribute { get; set; }

        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }

    }
}
