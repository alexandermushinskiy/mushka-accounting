using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Supplier;

namespace Mushka.WebApi.Resolvers
{
    public class SupplierResponseResolver :
        IValueResolver<ValidationResponse<Supplier>, SupplierResponseModel, SupplierModel>,
        IValueResolver<ValidationResponse<IEnumerable<Supplier>>, SuppliersResponseModel, IEnumerable<SupplierModel>>
    {
        public SupplierModel Resolve(
            ValidationResponse<Supplier> source,
            SupplierResponseModel destination,
            SupplierModel destMember,
            ResolutionContext context) => source.Result == null ? null : Mapper.Map<Supplier, SupplierModel>(source.Result);

        public IEnumerable<SupplierModel> Resolve(
            ValidationResponse<IEnumerable<Supplier>> source,
            SuppliersResponseModel destination,
            IEnumerable<SupplierModel> destMember,
            ResolutionContext context) => source.Result?.Select(Mapper.Map<Supplier, SupplierModel>);
    }
}