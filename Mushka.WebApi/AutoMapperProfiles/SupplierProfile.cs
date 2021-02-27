using System.Collections.Generic;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Supplier.Describe;
using Mushka.WebApi.ClientModels.Supplier.Modify;
using Mushka.WebApi.ClientModels.Supplier.Search;
using Mushka.WebApi.Resolvers.Suppliers;

namespace Mushka.WebApi.AutoMapperProfiles
{
    public class SupplierProfile : Profile
    {
        public SupplierProfile()
        {
            CreateMap<CreateSupplierRequestModel, Supplier>().ConvertUsing<CreateSupplierRequestConverter>();
            CreateMap<UpdateSupplierRequestModel, Supplier>().ConvertUsing<UpdateSupplierRequestConverter>();

            CreateMap<OperationResult<IEnumerable<Supplier>>, SearchSuppliersResponseModel>()
                .ConvertUsing<SearchSuppliersResponseConverter>();

            CreateMap<OperationResult<Supplier>, DescribeSupplierResponseModel>()
                .ConvertUsing<DescribeSupplierResponseConverter>();
        }
    }
}