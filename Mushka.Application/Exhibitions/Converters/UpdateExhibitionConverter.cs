using System;
using System.Collections.Generic;
using System.Linq;
using Mushka.Application.Exhibitions.Commands;
using Mushka.Domain.Entities;

namespace Mushka.Application.Exhibitions.Converters;

public sealed class UpdateExhibitionConverter : IUpdateExhibitionConverter
{
    public Exhibition ToExhibition(UpdateExhibitionCommand command)
    {
        return new Exhibition
        {
            Id = command.Id,
            Name = command.Exhibition.Name,
            FromDate = command.Exhibition.FromDate,
            ToDate = command.Exhibition.ToDate,
            City = command.Exhibition.City,
            ParticipationCost = command.Exhibition.ParticipationCost,
            ParticipationCostMethod = command.Exhibition.ParticipationCostMethod,
            AccommodationCost = command.Exhibition.AccommodationCost,
            AccommodationCostMethod = command.Exhibition.AccommodationCostMethod,
            FareCost = command.Exhibition.FareCost,
            FareCostMethod = command.Exhibition.FareCostMethod,
            Profit = command.Exhibition.Profit,
            Notes = command.Exhibition.Notes,
            Products = ToProducts(command.Id, command.Products)
        };
    }

    private static List<ExhibitionProduct> ToProducts(Guid exhibitionId, IEnumerable<ExhibitionProductData> products) =>
        (products ?? Enumerable.Empty<ExhibitionProductData>())
            .Select(product => new ExhibitionProduct
            {
                ExhibitionId = exhibitionId,
                ProductId = product.ProductId,
                Quantity = product.Quantity,
                UnitPrice = product.UnitPrice,
                CostPrice = product.CostPrice
            })
            .ToList();
}
