using System.ComponentModel.DataAnnotations;

namespace Mushka.Api.ClientModels.CorporateOrder;

public class ValidateCorporateOrderNumberRequestModel
{
    [Required]
    public string OrderNumber { get; set; }
}
