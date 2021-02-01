using System.Collections.Generic;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Category;
using Mushka.WebApi.ClientModels.Category.GetById;
using Mushka.WebApi.ClientModels.Category.Search;
using Mushka.WebApi.Resolvers.Categories;

namespace Mushka.WebApi.AutoMapperProfiles
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<OperationResult<IEnumerable<Category>>, SearchCategoriesResponseModel>()
                .ConvertUsing<SearchCategoriesResponseConverter>();

            CreateMap<Category, CategoryModel>();
            CreateMap<CategoryRequestModel, Category>().ConvertUsing<CategoryRequestConverter>();

            CreateMap<OperationResult<Category>, CategoryResponseModel>()
                .ConvertUsing<CategoryResponseConverter>();
        }
    }
}