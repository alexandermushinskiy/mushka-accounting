using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Product
{
    public class ProductResponseModel : ResponseModelBase
    {
        public ProductModel Data { get; set; }
    }

    public class ProductsResponseModel : ResponseModelBase
    {
        public IEnumerable<ProductModel> Data { get; set; }
    }
}