using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Category.GetById;

namespace Mushka.WebApi.Resolvers.Categories
{
    public class CategoryResponseConverter : ITypeConverter<OperationResult<Category>, CategoryResponseModel>
    {
        public CategoryResponseModel Convert(OperationResult<Category> source, CategoryResponseModel destination, ResolutionContext context)
        {
            return new CategoryResponseModel
            {
                Category = Mapper.Map<Category, CategoryModel>(source.Data)
            };
        }
    }
}