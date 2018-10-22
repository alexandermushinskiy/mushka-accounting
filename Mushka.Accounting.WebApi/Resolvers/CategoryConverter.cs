using AutoMapper;
using Mushka.Accounting.Domain.Entities;
using Mushka.Accounting.WebApi.ClientModels.Category;

namespace Mushka.Accounting.WebApi.Resolvers
{
    public class CategoryConverter : ITypeConverter<Category, CategoryModel>
    {
        public CategoryModel Convert(Category source, CategoryModel destination, ResolutionContext context) =>
            new CategoryModel
            {
                Id = source.Id,
                Name = source.Name
            };
    }
}