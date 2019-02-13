using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Exhibition
{
    public class ExhibitionProductsResponseModel : ResponseModelBase
    {
        public IEnumerable<ExhibitionProductModel> Data { get; set; }
    }
}