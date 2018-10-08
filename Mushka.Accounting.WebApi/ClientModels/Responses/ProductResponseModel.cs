using Mushka.Accounting.Domain.Entities;

namespace Mushka.Accounting.WebApi.ClientModels.Responses
{
    public class ProductResponseModel : ResourceResponseModelBase
    {
        public Product Data { get; set; }
    }
}