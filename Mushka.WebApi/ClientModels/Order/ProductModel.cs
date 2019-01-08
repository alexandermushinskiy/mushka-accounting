using System;

namespace Mushka.WebApi.ClientModels.Order
{
    public class ProductModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string VendorCode { get; set; }

        public string Size { get; set; }
    }
}