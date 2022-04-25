using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Customer
{
    public class CustomersResponseModel
    {
        public IEnumerable<CustomerModel> Customers { get; set; }
    }
}