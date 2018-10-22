using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Accounting.Core.Extensibility.Validation;
using Mushka.Accounting.Core.Validation;
using Mushka.Accounting.Core.Validation.Enums;
using Mushka.Accounting.Domain.Entities;
using Mushka.Accounting.WebApi.ClientModels;
using Mushka.Accounting.WebApi.ClientModels.Category;
using Mushka.Accounting.WebApi.ClientModels.Product;
using Mushka.Accounting.WebApi.Resolvers;

namespace Mushka.Accounting.WebApi
{
    public class AccountingAutoMapperProfile : Profile
    {
        public AccountingAutoMapperProfile()
        {
            CreateMap<ValidationStatusType, int?>().ConvertUsing<StatusCodeTypeConverter>();

            CreateMap<IValidationResult, MessageResponseModel>()
                .ForMember(dest => dest.LevelType, opts => opts.MapFrom(src => src.Level))
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.Message, opts => opts.MapFrom(src => src.Message));
            
            // Products
            CreateMap<ProductRequestModel, Product>().ConvertUsing<ProductRequestConverter>();

            CreateMap<Product, ProductModel>().ConvertUsing<ProductConverter>();

            CreateMap<ValidationResponse<Product>, ProductResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<ProductResponseResolver>())
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.ValidationResults));

            CreateMap<ValidationResponse<IEnumerable<Product>>, ProductsResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<ProductResponseResolver>())
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.ValidationResults));
            //-------------------------

            // Category
            CreateMap<CategoryRequestModel, Category>()
                .ForMember(dest => dest.Id, opt => opt.ResolveUsing<GuidResolver>())
                .ForMember(dest => dest.Products, opt => opt.Ignore());

            CreateMap<Category, CategoryModel>().ConvertUsing<CategoryConverter>();

            CreateMap<ValidationResponse<Category>, CategoryResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<CategoryResponseResolver>())
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.ValidationResults));

            CreateMap<ValidationResponse<IEnumerable<Category>>, CategoriesResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<CategoryResponseResolver>())
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.ValidationResults));
            //-------------------------


            

            //CreateMap<ValidationResponse<IEnumerable<Supplier>>, SuppliersResponseModel>()
            //    .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Result))
            //    .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.ValidationResults));

            //CreateMap<ValidationResponse<Supplier>, SupplierResponseModel>()
            //    .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Result))
            //    .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.ValidationResults));

            //CreateMap<SupplierRequestModel, Supplier>()
            //    .ForMember(dest => dest.Id, opt => opt.Ignore())
            //    .ForMember(dest => dest.CreatedOn, opt => opt.Ignore())
            //    .ForMember(dest => dest.Deliveries, opt => opt.Ignore());
        }
    }
}