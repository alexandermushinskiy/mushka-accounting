using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Product
{
    public class SelectProductsResponseModel : ResponseModelBase
    {
        public IEnumerable<SelectProductModel> Data { get; set; }
    }
}
