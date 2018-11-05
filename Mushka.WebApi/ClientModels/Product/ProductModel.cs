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

        public IEnumerable<Guid> Sizes { get; set; }
    }
}