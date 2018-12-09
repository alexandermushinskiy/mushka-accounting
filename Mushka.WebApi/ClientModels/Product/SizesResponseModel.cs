using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Product
{
    public class SizesResponseModel : ResponseModelBase
    {
        public IEnumerable<SizeModel> Data { get; set; }
    }
}