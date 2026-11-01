using System.ComponentModel.DataAnnotations;

namespace Mushka.Api.ClientModels.Order;

public class ValidateOrderNumberRequestModel
{
    [Required]
    public string OrderNumber { get; set; }
}
