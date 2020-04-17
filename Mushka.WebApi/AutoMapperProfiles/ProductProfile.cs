using System.Collections.Generic;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Dto;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Category;
using Mushka.WebApi.ClientModels.Product;
using Mushka.WebApi.Resolvers;
using CategoryModel = Mushka.WebApi.ClientModels.Category.CategoryModel;
using ProductModel = Mushka.WebApi.ClientModels.Product.ProductModel;

namespace Mushka.WebApi.AutoMapperProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateCategoryMaps();
            CreateProductMaps();
        }

        private void CreateCategoryMaps()
        {
            CreateMap<Category, CategoryModel>().ConvertUsing<CategoryConverter>();
            CreateMap<CategoryRequestModel, Category>().ConvertUsing<CategoryRequestConverter>();

            CreateMap<ValidationResponse<Category>, CategoryResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<CategoryResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<Category>>, CategoriesResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<CategoryResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));
        }

        private void CreateProductMaps()
        {
            CreateMap<ProductRequestModel, Product>().ConvertUsing<ProductRequestConverter>();
            CreateMap<Product, ProductModel>().ConvertUsing<ProductConverter>();
            CreateMap<Product, ProductListModel>().ConvertUsing<ProductListConverter>();

            CreateMap<ValidationResponse<Product>, ProductResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<ProductResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<Product>>, ProductListResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<ProductListResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<Product>>, SelectProductsResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<SelectProductsResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<Size>>, SizesResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<SizeResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<ProductCostPrice>, CostPriceResponseModel>()
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }))
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => new CostPriceModel { CostPrice = src.Result.CostPrice }));
        }
    }
}