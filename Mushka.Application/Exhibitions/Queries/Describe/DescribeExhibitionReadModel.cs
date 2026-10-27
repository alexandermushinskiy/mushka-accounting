using System;
using System.Collections.Generic;
using Mushka.Domain.Entities;

namespace Mushka.Application.Exhibitions.Queries.Describe;

public sealed class DescribeExhibitionReadModel
{
    public DescribeExhibitionDetailsReadModel Exhibition { get; set; }

    public IEnumerable<DescribeExhibitionProductReadModel> Products { get; set; }
}

public sealed class DescribeExhibitionDetailsReadModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public string City { get; set; }

    public decimal ParticipationCost { get; set; }

    public PaymentMethod ParticipationCostMethod { get; set; }

    public decimal? AccommodationCost { get; set; }

    public PaymentMethod? AccommodationCostMethod { get; set; }

    public decimal? FareCost { get; set; }

    public PaymentMethod? FareCostMethod { get; set; }

    public decimal Profit { get; set; }

    public string Notes { get; set; }
}

public sealed class DescribeExhibitionProductReadModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string VendorCode { get; set; }

    public string SizeName { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal CostPrice { get; set; }
}
