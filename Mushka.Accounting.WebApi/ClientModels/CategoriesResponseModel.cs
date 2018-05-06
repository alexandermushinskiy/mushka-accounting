using System.Collections.Generic;
using Mushka.Accounting.Domain.Entities;

namespace Mushka.Accounting.WebApi.ClientModels
{
    public class CategoriesResponseModel : ResourceResponseModelBase
    {
        public IEnumerable<Category> Data { get; set; }
    }
}