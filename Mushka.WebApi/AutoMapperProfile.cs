using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Mushka.Core.Validation;
using Mushka.Core.Validation.Enums;
using Mushka.Domain.Dto;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels;
using Mushka.WebApi.ClientModels.Analytics;
using Mushka.WebApi.ClientModels.Category;
using Mushka.WebApi.ClientModels.CorporateOrder;
using Mushka.WebApi.ClientModels.Customer;
using Mushka.WebApi.ClientModels.Exhibition;
using Mushka.WebApi.ClientModels.Expenses;
using Mushka.WebApi.ClientModels.Order;
using Mushka.WebApi.ClientModels.Product;
using Mushka.WebApi.ClientModels.Supplier;
using Mushka.WebApi.ClientModels.Supply;
using Mushka.WebApi.Resolvers;
using CategoryModel = Mushka.WebApi.ClientModels.Category.CategoryModel;
using ProductModel = Mushka.WebApi.ClientModels.Product.ProductModel;

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

            CreateMap<ValidationResponse<bool>, ValidationResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => new ValidationRModel(src.Result)))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }))
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status));

            // Products
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
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] {src.ValidationResult.Message}))
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => new CostPriceModel { CostPrice = src.Result.CostPrice }));
            //-------------------------

            // Category
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
            //-------------------------

            // Supply
            CreateMap<SupplyRequestModel, Supply>().ConvertUsing<SupplyRequestConverter>();
            CreateMap<Supply, SupplyModel>().ConvertUsing<SupplyConverter>();

            CreateMap<ValidationResponse<Supply>, SupplyResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<SupplyResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            //CreateMap<ValidationResponse<IEnumerable<Supply>>, SuppliesResponseModel>()
            //    .ForMember(dest => dest.Data, opt => opt.ResolveUsing<SupplyResponseResolver>())
            //    .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
            //    .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<Supply>>, SuppliesListResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<SuppliesListResponseResolver>())
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
            CreateMap<OrderProduct, OrderProductModel>().ConvertUsing<OrderConverter>();

            CreateMap<ValidationResponse<Order>, OrderResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<OrderResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            //CreateMap<ValidationResponse<IEnumerable<Order>>, OrdersResponseModel>()
            //    .ForMember(dest => dest.Data, opt => opt.ResolveUsing<OrderResponseResolver>())
            //    .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
            //    .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<Order>>, OrdersListResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<OrdersListResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<OrderProduct>>, OrderProductsResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<OrderProductResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));
            //-------------------------

            // Customer
            CreateMap<Customer, CustomerModel>().ConvertUsing<CustomerConverter>();

            CreateMap<ValidationResponse<IEnumerable<Customer>>, CustomersResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<CustomerResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));
            //-------------------------

            // Exhibition
            CreateMap<ExhibitionRequestModel, Exhibition>().ConvertUsing<ExhibitionRequestConverter>();
            CreateMap<Exhibition, ExhibitionModel>().ConvertUsing<ExhibitionConverter>();
            CreateMap<ExhibitionProduct, ExhibitionProductModel>().ConvertUsing<ExhibitionConverter>();

            CreateMap<ValidationResponse<Exhibition>, ExhibitionResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<ExhibitionResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<Exhibition>>, ExhibitionsResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<ExhibitionResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<Exhibition>>, ExhibitionsListResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<ExhibitionsListResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<ExhibitionProduct>>, ExhibitionProductsResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<ExhibitionProductResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));
            //-------------------------

            // Expenses
            CreateMap<ExpenseRequestModel, Expense>().ConvertUsing<ExpenseRequestConverter>();
            CreateMap<Expense, ExpenseModel>().ConvertUsing<ExpenseConverter>();
            
            CreateMap<ValidationResponse<Expense>, ExpenseResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<ExpenseResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<Expense>>, ExpensesResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<ExpenseResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));
            //-------------------------

            // Analytics
            CreateMap<ValidationResponse<Balance>, BalanceResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Result))
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<PopularProduct>>, PopularProductsResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Result))
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<PopularCity>>, PopularCitiesResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Result))
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<OrdersCount>>, OrdersCountResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Result))
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<SoldProductsCount>>, SoldProductsCountResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Result))
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));
            //-------------------------

            // Corporate orders
            CreateMap<CorporateOrderRequestModel, CorporateOrder>().ConvertUsing<CorporateOrderRequestConverter>();
            CreateMap<CorporateOrder, CorporateOrderModel>().ConvertUsing<CorporateOrderConverter>();

            CreateMap<ValidationResponse<CorporateOrder>, CorporateOrderResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<CorporateOrderResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            CreateMap<ValidationResponse<IEnumerable<CorporateOrder>>, CorporateOrdersListResponseModel>()
                .ForMember(dest => dest.Data, opt => opt.ResolveUsing<CorporateOrdersListResponseResolver>())
                .ForMember(dest => dest.StatusCode, opt => opt.MapFrom(src => src.ValidationResult.Status))
                .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => new[] { src.ValidationResult.Message }));

            //-------------------------
        }
    }
}