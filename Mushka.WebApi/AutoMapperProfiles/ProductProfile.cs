using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Dto;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Product;
using Mushka.WebApi.ClientModels.Product.GetById;
using Mushka.WebApi.ClientModels.Product.GetCostPrice;
using Mushka.WebApi.Resolvers;
using Mushka.WebApi.Resolvers.Products;
using ProductModel = Mushka.WebApi.ClientModels.Product.ProductModel;

namespace Mushka.WebApi.AutoMapperProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateProductMaps();
        }

        private void CreateProductMaps()
        {
            CreateMap<ProductRequestModel, Product>().ConvertUsing<ProductRequestConverter>();
            CreateMap<Product, ProductModel>().ConvertUsing<ProductConverter>();

            CreateMap<OperationResult<Product>, ProductResponseModel>()
                .ConvertUsing<ProductResponseConverter>();

            CreateMap<OperationResult<IEnumerable<Product>>, SearchProductsResponseModel>()
                .ConvertUsing<SearchProductsResponseConverter>();

            CreateMap<OperationResult<IEnumerable<Product>>, SelectProductsResponseModel>()
                .ConvertUsing<SelectProductsResponseConverter>();

            CreateMap<OperationResult<IEnumerable<Size>>, SizesResponseModel>()
                .ConvertUsing<SizesResponseConverter>();

            CreateMap<OperationResult<ProductCostPrice>, CostPriceResponseModel>()
                .ForMember(dest => dest.CostPrice, opt => opt.MapFrom(src => src.Data.CostPrice));
        }
    }
}