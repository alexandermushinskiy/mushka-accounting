using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Accounting.Core.Validation;
using Mushka.Accounting.Domain.Entities;
using Mushka.Accounting.WebApi.ClientModels.Category;

namespace Mushka.Accounting.WebApi.Resolvers
{
    public class CategoryResponseResolver :
        IValueResolver<ValidationResponse<Category>, CategoryResponseModel, CategoryModel>,
        IValueResolver<ValidationResponse<IEnumerable<Category>>, CategoriesResponseModel, IEnumerable<CategoryModel>>
    {
        public CategoryModel Resolve(
            ValidationResponse<Category> source,
            CategoryResponseModel destination,
            CategoryModel destMember,
            ResolutionContext context) => source.Result == null ? null : Mapper.Map<Category, CategoryModel>(source.Result);

        public IEnumerable<CategoryModel> Resolve(
            ValidationResponse<IEnumerable<Category>> source,
            CategoriesResponseModel destination,
            IEnumerable<CategoryModel> destMember,
            ResolutionContext context) => source.Result?.Select(Mapper.Map<Category, CategoryModel>);
    }
}