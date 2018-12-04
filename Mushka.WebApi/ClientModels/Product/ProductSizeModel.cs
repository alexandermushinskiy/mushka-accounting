using System;

namespace Mushka.WebApi.ClientModels.Product
{
    public class ProductSizeModel
    {
        public Guid Id { get; set; }

        public string Size { get; set; }

        public int Quantity { get; set; }
    }
}