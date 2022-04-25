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

        public bool IsArchival { get; set; }
    }
}