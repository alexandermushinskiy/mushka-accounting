using System.Collections.Generic;
using Mushka.Domain.Dto;

namespace Mushka.WebApi.ClientModels.Analytics
{
    public class OrdersCountResponseModel : ResponseModelBase
    {
        public IEnumerable<OrdersCount> Data { get; set; }
    }
}