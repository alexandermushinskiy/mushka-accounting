using System.Collections.Generic;

namespace Mushka.Accounting.WebApi.ClientModels.Category
{
    public class CategoriesResponseModel : ResponseModelBase
    {
        public IEnumerable<CategoryModel> Data { get; set; }
    }
}