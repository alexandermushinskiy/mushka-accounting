using System.Linq;
using Mushka.Application.Exhibitions.Queries.Describe;
using Mushka.Domain.Entities;

namespace Mushka.Application.Exhibitions.Converters;

public sealed class DescribeExhibitionConverter : IDescribeExhibitionConverter
{
    public DescribeExhibitionReadModel ToReadModel(Exhibition exhibition) =>
        new DescribeExhibitionReadModel
        {
            Exhibition = ToExhibition(exhibition),
            Products = exhibition.Products?.Select(ToProduct) ?? Enumerable.Empty<DescribeExhibitionProductReadModel>()
        };

    private static DescribeExhibitionDetailsReadModel ToExhibition(Exhibition exhibition) =>
        new DescribeExhibitionDetailsReadModel
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

    private static DescribeExhibitionProductReadModel ToProduct(ExhibitionProduct exhibitionProduct) =>
        new DescribeExhibitionProductReadModel
        {
            Id = exhibitionProduct.ProductId,
            Name = exhibitionProduct.Product?.Name,
            VendorCode = exhibitionProduct.Product?.VendorCode,
            SizeName = exhibitionProduct.Product?.Size?.Name,
            Quantity = exhibitionProduct.Quantity,
            UnitPrice = exhibitionProduct.UnitPrice,
            CostPrice = exhibitionProduct.CostPrice
        };
}
