using System.Collections.Generic;
using Mushka.Domain.Dto;

namespace Mushka.WebApi.ClientModels.Analytics
{
    public class SoldProductsCountResponseModel : ResponseModelBase
    {
        public IEnumerable<SoldProductsCount> Data { get; set; }
    }
}