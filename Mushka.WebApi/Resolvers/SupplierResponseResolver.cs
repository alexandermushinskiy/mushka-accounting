using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Supplier;

namespace Mushka.WebApi.Resolvers
{
    public class SupplierResponseResolver :
        IValueResolver<OperationResult<Supplier>, SupplierResponseModel, SupplierModel>,
        IValueResolver<OperationResult<IEnumerable<Supplier>>, SuppliersListResponseModel, IEnumerable<SupplierModel>>
    {
        public SupplierModel Resolve(
            OperationResult<Supplier> source,
            SupplierResponseModel destination,
            SupplierModel destMember,
            ResolutionContext context) => source.Data == null ? null : Mapper.Map<Supplier, SupplierModel>(source.Data);

        public IEnumerable<SupplierModel> Resolve(
            OperationResult<IEnumerable<Supplier>> source,
            SuppliersListResponseModel destination,
            IEnumerable<SupplierModel> destMember,
            ResolutionContext context) => source.Data?.Select(Mapper.Map<Supplier, SupplierModel>);
    }
}