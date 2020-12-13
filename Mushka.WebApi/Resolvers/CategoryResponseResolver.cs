﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Category;

namespace Mushka.WebApi.Resolvers
{
    public class CategoryResponseResolver :
        IValueResolver<OperationResult<Category>, CategoryResponseModel, CategoryModel>, 
        IValueResolver<OperationResult<IEnumerable<Category>>, CategoriesResponseModel, IEnumerable<CategoryModel>>
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

        public CategoryModel Resolve(
            OperationResult<Category> source,
            CategoryResponseModel destination,
            CategoryModel destMember,
            ResolutionContext context) => source.Data == null ? null : Mapper.Map<Category, CategoryModel>(source.Data);

        public IEnumerable<CategoryModel> Resolve(
            OperationResult<IEnumerable<Category>> source,
            CategoriesResponseModel destination,
            IEnumerable<CategoryModel> destMember,
            ResolutionContext context) => source.Data?.Select(Mapper.Map<Category, CategoryModel>);
    }
}