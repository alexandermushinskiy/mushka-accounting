using System;

namespace Mushka.WebApi.ClientModels.Product
{
    public class ProductModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string VendorCode { get; set; }

        public decimal? RecommendedPrice { get; set; }

        public DateTime CreatedOn { get; set; }
        
        public Guid CategoryId { get; set; }

        public CategoryModel Category { get; set; }

        public SizeModel Size { get; set; }

        public bool IsArchival { get; set; }
    }
}