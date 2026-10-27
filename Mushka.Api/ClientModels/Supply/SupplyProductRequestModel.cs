using System;
using Mushka.Api.Filters;

namespace Mushka.Api.ClientModels.Supply;

public class SupplyProductRequestModel
{
    [RequireNonDefault]
    public Guid ProductId { get; set; }

    [RequireNonDefault]
    public int Quantity { get; set; }

    [RequireNonDefault]
    public decimal UnitPrice { get; set; }

    [RequireNonDefault]
    public decimal CostPrice { get; set; }
}
