using System.Collections.Generic;

namespace Mushka.Accounting.WebApi.ClientModels.Product
{
    public class ProductResponseModel : ResourceResponseModelBase
    {
        public ProductModel Data { get; set; }
    }

    public class ProductsResponseModel : ResourceResponseModelBase
    {
        public IEnumerable<ProductModel> Data { get; set; }
    }
}