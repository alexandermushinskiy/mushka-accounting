using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Product
{
    public class SizesResponseModel
    {
        public IEnumerable<SizeModel> Items { get; set; }
    }
}