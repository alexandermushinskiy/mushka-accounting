using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Exhibition
{
    public class ExhibitionsListResponseModel : ResponseModelBase
    {
        public IEnumerable<ExhibitionsListModel> Data { get; set; }
    }
}