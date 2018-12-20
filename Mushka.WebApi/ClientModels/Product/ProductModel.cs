using System;
using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Product
{
    public class ProductModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid CategoryId { get; set; }

        public CategoryModel Category { get; set; }

        public int? DeliveriesCount { get; set; }

        public DateTime? LastDeliveryDate { get; set; }

        public int? LastDeliveryCount { get; set; }

        public IEnumerable<ProductSizeModel> Sizes { get; set; }
    }
}