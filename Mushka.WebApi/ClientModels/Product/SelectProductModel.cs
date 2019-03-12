using System;

namespace Mushka.WebApi.ClientModels.Product
{
    public class SelectProductModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string VendorCode { get; set; }

        public decimal? RecommendedPrice { get; set; }

        public int Quantity { get; set; }

        public string CategoryName { get; set; }

        public string SizeName { get; set; }
        
        public bool IsArchival { get; set; }
    }
}