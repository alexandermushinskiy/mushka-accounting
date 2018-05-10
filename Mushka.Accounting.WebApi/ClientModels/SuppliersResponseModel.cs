using System.Collections.Generic;
using Mushka.Accounting.Domain.Entities;

namespace Mushka.Accounting.WebApi.ClientModels
{
    public class SuppliersResponseModel : ResourceResponseModelBase
    {
        public IEnumerable<Supplier> Data { get; set; }
    }
}