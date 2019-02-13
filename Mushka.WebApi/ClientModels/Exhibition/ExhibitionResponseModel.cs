using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Exhibition
{
    public class ExhibitionResponseModel : ResponseModelBase
    {
        public ExhibitionModel Data { get; set; }
    }

    public class ExhibitionsResponseModel : ResponseModelBase
    {
        public IEnumerable<ExhibitionModel> Data { get; set; }
    }
}