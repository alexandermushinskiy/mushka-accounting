using System.Linq;
using AutoMapper;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Exhibition;

namespace Mushka.WebApi.Resolvers
{
    public class ExhibitionConverter : ITypeConverter<Exhibition, ExhibitionModel>, ITypeConverter<ExhibitionProduct, ExhibitionProductModel>
    {
        public ExhibitionModel Convert(Exhibition source, ExhibitionModel destination, ResolutionContext context) =>
            new ExhibitionModel
            {
                Id = source.Id,
                Name = source.Name,
                FromDate = source.FromDate,
                ToDate = source.ToDate,
                City = source.City,
                ParticipationCost = source.ParticipationCost,
                ParticipationCostMethod = source.ParticipationCostMethod,
                AccommodationCost = source.AccommodationCost,
                AccommodationCostMethod = source.AccommodationCostMethod,
                FareCost = source.FareCost,
                FareCostMethod = source.FareCostMethod,
                Profit = source.Profit,
                Notes = source.Notes,
                Products = source.Products.Select(CreateOrderProductModel)
            };


        public ExhibitionProductModel Convert(ExhibitionProduct source, ExhibitionProductModel destination, ResolutionContext context) =>
            CreateOrderProductModel(source);

        private static ExhibitionProductModel CreateOrderProductModel(ExhibitionProduct orderProduct) =>
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