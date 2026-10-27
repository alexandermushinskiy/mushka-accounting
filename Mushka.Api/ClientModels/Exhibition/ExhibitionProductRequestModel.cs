using System;
using Mushka.Api.Filters;

namespace Mushka.Api.ClientModels.Exhibition;

public class ExhibitionProductRequestModel
{
    [RequireNonDefault]
    public Guid ProductId { get; set; }

    [RequireNonDefault]
    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    [RequireNonDefault]
    public decimal CostPrice { get; set; }
}
