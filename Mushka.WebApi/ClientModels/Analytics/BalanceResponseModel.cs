using Mushka.Domain.Dto;

namespace Mushka.WebApi.ClientModels.Analytics
{
    public class BalanceResponseModel : ResponseModelBase
    {
        public Balance Data { get; set; }
    }
}