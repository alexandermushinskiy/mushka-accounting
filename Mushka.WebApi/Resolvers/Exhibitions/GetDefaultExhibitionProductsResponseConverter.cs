using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Exhibition.GetDefaultProducts;

namespace Mushka.WebApi.Resolvers.Exhibitions
{
    public class GetDefaultExhibitionProductsResponseConverter :
        ITypeConverter<OperationResult<IEnumerable<ExhibitionProduct>>, GetDefaultExhibitionProductsResponseModel>
    {
        public GetDefaultExhibitionProductsResponseModel Convert(
            OperationResult<IEnumerable<ExhibitionProduct>> source,
            GetDefaultExhibitionProductsResponseModel destination,
            ResolutionContext context)
        {
            return new GetDefaultExhibitionProductsResponseModel
            {
                Products = source.Data.Select(ConvertToExhibitionProductModel)
            };
        }

        private static ExhibitionProductModel ConvertToExhibitionProductModel(ExhibitionProduct orderProduct) =>
            new ExhibitionProductModel
            {
                Id = orderProduct.ProductId,
                Name = orderProduct.Product?.Name,
                VendorCode = orderProduct.Product?.VendorCode,
                SizeName = orderProduct.Product?.Size?.Name,
                Quantity = orderProduct.Quantity,
                UnitPrice = orderProduct.UnitPrice,
                CostPrice = orderProduct.CostPrice
            };
    }
}