using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Product
{
    public class SelectProductsResponseModel
    {
        public int Total { get; set; }
        public IEnumerable<SelectProductModel> Items { get; set; }
    }
}