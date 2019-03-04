using System;
using System.Linq;
using AutoMapper;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Exhibition;
using Mushka.WebApi.Extensions;

namespace Mushka.WebApi.Resolvers
{
    public class ExhibitionRequestConverter : ITypeConverter<ExhibitionRequestModel, Exhibition>
    {
        private readonly IGuidProvider guidProvider;

        public ExhibitionRequestConverter(IGuidProvider guidProvider)
        {
            this.guidProvider = guidProvider;
        }

        public Exhibition Convert(ExhibitionRequestModel source, Exhibition destination, ResolutionContext context)
        {
            var exhibitionId = context.GetId() ?? guidProvider.NewGuid();

            return new Exhibition
            {
                Id = exhibitionId,
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
                Products = source.Products.Select(prod => CreateExhibitionProduct(exhibitionId, prod)).ToList()
            };
        }

        private static ExhibitionProduct CreateExhibitionProduct(Guid exhibitionId, ExhibitionProductRequestModel requestModel) =>
            new ExhibitionProduct
            {
                ExhibitionId = exhibitionId,
                ProductId = requestModel.ProductId,
                Quantity = requestModel.Quantity,
                UnitPrice = requestModel.UnitPrice,
                CostPrice = requestModel.CostPrice
            };
    }
}