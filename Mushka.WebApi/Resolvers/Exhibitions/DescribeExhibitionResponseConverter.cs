using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Exhibition.Describe;

namespace Mushka.WebApi.Resolvers.Exhibitions
{
    public class DescribeExhibitionResponseConverter :
        ITypeConverter<OperationResult<Exhibition>, DescribeExhibitionResponseModel>
    {
        public DescribeExhibitionResponseModel Convert(
            OperationResult<Exhibition> source,
            DescribeExhibitionResponseModel destination,
            ResolutionContext context)
        {
            return new DescribeExhibitionResponseModel
            {
                Exhibition = ConvertToDescribeExhibition(source.Data),
                Products = source.Data.Products.Select(ConvertToDescribeProduct)
            };
        }

        private static DescribeExhibitionModel ConvertToDescribeExhibition(Exhibition exhibition) =>
            new DescribeExhibitionModel
            {
                Id = exhibition.Id,
                Name = exhibition.Name,
                FromDate = exhibition.FromDate,
                ToDate = exhibition.ToDate,
                City = exhibition.City,
                ParticipationCost = exhibition.ParticipationCost,
                ParticipationCostMethod = exhibition.ParticipationCostMethod,
                AccommodationCost = exhibition.AccommodationCost,
                AccommodationCostMethod = exhibition.AccommodationCostMethod,
                FareCost = exhibition.FareCost,
                FareCostMethod = exhibition.FareCostMethod,
                Profit = exhibition.Profit,
                Notes = exhibition.Notes
            };

        private static DescribeProductModel ConvertToDescribeProduct(ExhibitionProduct orderProduct) =>
            new DescribeProductModel
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