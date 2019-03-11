using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Product
{
    public class ProductListResponseModel : ResponseModelBase
    {
        public IEnumerable<ProductListModel> Data { get; set; }
    }
}