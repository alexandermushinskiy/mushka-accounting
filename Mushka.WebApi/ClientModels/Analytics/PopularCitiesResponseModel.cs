using System.Collections.Generic;
using Mushka.Domain.Dto;

namespace Mushka.WebApi.ClientModels.Analytics
{
    public class PopularCitiesResponseModel : ResponseModelBase
    {
        public IEnumerable<PopularCity> Data { get; set; }
    }
}