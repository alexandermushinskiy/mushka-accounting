using System.Collections.Generic;

namespace Mushka.Accounting.WebApi.ClientModels.Category
{
    public class CategoriesResponseModel : ResourceResponseModelBase
    {
        public IEnumerable<CategoryModel> Data { get; set; }
    }
}