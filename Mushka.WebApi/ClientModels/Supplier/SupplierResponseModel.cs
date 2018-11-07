using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Supplier
{
    public class SupplierResponseModel : ResponseModelBase
    {
        public SupplierModel Data { get; set; }
    }

    public class SuppliersResponseModel : ResponseModelBase
    {
        public IEnumerable<SupplierModel> Data { get; set; }
    }
}