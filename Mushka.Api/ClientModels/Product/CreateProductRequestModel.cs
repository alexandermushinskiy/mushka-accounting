using System;
using System.ComponentModel.DataAnnotations;

namespace Mushka.Api.ClientModels.Product;

public class CreateProductRequestModel
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string VendorCode { get; set; }

    public decimal? RecommendedPrice { get; set; }

    [Required]
    public Guid CategoryId { get; set; }

    public Guid? SizeId { get; set; }

    public bool IsArchival { get; set; }
}
