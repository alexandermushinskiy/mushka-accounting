using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Mushka.Core.Validation;
using Mushka.Core.Validation.Enums;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels;
using Mushka.WebApi.ClientModels.Category;
using Mushka.WebApi.ClientModels.Delivery;
using Mushka.WebApi.ClientModels.Order;
using Mushka.WebApi.ClientModels.Product;
using Mushka.WebApi.ClientModels.Supplier;
using Mushka.WebApi.Resolvers;

namespace Mushka.WebApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ValidationStatusType, int?>().ConvertUsing<StatusCodeTypeConverter>();

            CreateMap<IEnumerable<string>, ResponseModelBase>()
                .ForMember(dest => dest.StatusCode, opt => opt.UseValue(StatusCodes.Status400BadRequest))
                .ForMember(dest => dest.Messages, opts => opts.MapFrom(src => src));

            CreateMap<ValidationResponse, DeleteResponseModel>()
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }))
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status));

            // Products
            CreateMap<ProductRequestModel, Product>().ConvertUsing<ProductRequestConverter>();

            CreateMap<Product, ProductModel>().ConvertUsing<ProductConverter>();

            CreateMap<ValidationResponse<Product>, ProductResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<ProductResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<Product>>, ProductsResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<ProductResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<Size>>, SizesResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<SizeResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));
            //-------------------------

            // Category
            CreateMap<Category, CategoryModel>().ConvertUsing<CategoryConverter>();

            CreateMap<CategoryRequestModel, Category>()
                .ForMember(dest => dest.Id, opt => opt.ResolveUsing<GuidResolver>())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Products, opt => opt.Ignore());

            CreateMap<ValidationResponse<Category>, CategoryResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<CategoryResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<Category>>, CategoriesResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<CategoryResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));
            //-------------------------

            // Delivery
            CreateMap<DeliveryRequestModel, Delivery>().ConvertUsing<DeliveryRequestConverter>();
            
            CreateMap<Delivery, DeliveryModel>().ConvertUsing<DeliveryConverter>();

            CreateMap<ValidationResponse<Delivery>, DeliveryResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<DeliveryResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<Delivery>>, DeliveriesResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<DeliveryResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));
            //-------------------------

            // Supplier
            CreateMap<SupplierRequestModel, Supplier>().ConvertUsing<SupplierRequestConverter>();
            CreateMap<Supplier, SupplierModel>().ConvertUsing<SupplierConverter>();

            CreateMap<ValidationResponse<Supplier>, SupplierResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<SupplierResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<Supplier>>, SuppliersResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<SupplierResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));
            //-------------------------

            // Order
            CreateMap<OrderRequestModel, Order>().ConvertUsing<OrderRequestConverter>();
            CreateMap<Order, OrderModel>().ConvertUsing<OrderConverter>();
            
            CreateMap<ValidationResponse<Order>, OrderResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<OrderResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<Order>>, OrdersResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<OrderResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));
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