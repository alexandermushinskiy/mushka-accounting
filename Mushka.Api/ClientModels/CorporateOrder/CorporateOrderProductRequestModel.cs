using System.ComponentModel.DataAnnotations;
using Mushka.Api.Filters;

namespace Mushka.Api.ClientModels.CorporateOrder;

public class CorporateOrderProductRequestModel
{
    [Required]
    public string Name { get; set; }

    [RequireNonDefault]
    public int Quantity { get; set; }

    [RequireNonDefault]
    public decimal UnitPrice { get; set; }

    [RequireNonDefault]
    public decimal CostPrice { get; set; }
}
