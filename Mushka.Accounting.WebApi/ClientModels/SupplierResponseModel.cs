using Mushka.Accounting.Domain.Entities;

namespace Mushka.Accounting.WebApi.ClientModels
{
    public class SupplierResponseModel : ResourceResponseModelBase
    {
        public Supplier Data { get; set; }
    }
}