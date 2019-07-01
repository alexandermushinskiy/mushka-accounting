using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Customer
{
    public class CustomersResponseModel : ResponseModelBase
    {
        public IEnumerable<CustomerModel> Data { get; set; }
    }
}