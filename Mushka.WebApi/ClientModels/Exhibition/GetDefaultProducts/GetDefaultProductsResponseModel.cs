using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Exhibition.GetDefaultProducts
{
    public class GetDefaultExhibitionProductsResponseModel
    {
        public IEnumerable<ExhibitionProductModel> Products { get; set; }
    }
}