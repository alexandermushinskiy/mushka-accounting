using Mushka.Accounting.Domain.Entities;

namespace Mushka.Accounting.WebApi.ClientModels
{
    public class CategoryResponseModel : ResourceResponseModelBase
    {
        public Category Data { get; set; }
    }
}