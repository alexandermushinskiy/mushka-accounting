using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Category
{
    public class CategoryResponseModel : ResponseModelBase
    {
        public CategoryModel Data { get; set; }
    }

    public class CategoriesResponseModel : ResponseModelBase
    {
        public IEnumerable<CategoryModel> Data { get; set; }
    }
}