using System;
using System.Collections.Generic;
using System.Linq;
using Mushka.Api.ClientModels.Exhibition;
using Mushka.Application.Exhibitions.Commands;

namespace Mushka.Api.Converters.Exhibitions;

public sealed class ExhibitionRequestConverter : IExhibitionRequestConverter
{
    public CreateExhibitionCommand ToCreateCommand(ExhibitionRequestModel request) =>
        new CreateExhibitionCommand(ToExhibition(request), ToProducts(request.Products));

    public UpdateExhibitionCommand ToUpdateCommand(Guid id, ExhibitionRequestModel request) =>
        new UpdateExhibitionCommand(id, ToExhibition(request), ToProducts(request.Products));

    private static ExhibitionData ToExhibition(ExhibitionRequestModel request) =>
        new ExhibitionData(
            request.Name,
            request.FromDate,
            request.ToDate,
            request.City,
            request.ParticipationCost,
            request.ParticipationCostMethod,
            request.AccommodationCost,
            request.AccommodationCostMethod,
            request.FareCost,
            request.FareCostMethod,
            request.Profit,
            request.Notes);

    private static IReadOnlyList<ExhibitionProductData> ToProducts(IEnumerable<ExhibitionProductRequestModel> products) =>
        (products ?? Enumerable.Empty<ExhibitionProductRequestModel>())
            .Select(product => new ExhibitionProductData(
                product.ProductId,
                product.Quantity,
                product.UnitPrice,
                product.CostPrice))
            .ToList();
}
