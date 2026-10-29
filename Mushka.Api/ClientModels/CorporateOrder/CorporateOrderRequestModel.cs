using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mushka.Api.Filters;
using Mushka.Domain.Entities;

namespace Mushka.Api.ClientModels.CorporateOrder;

public class CorporateOrderRequestModel
{
    [RequireNonDefault]
    public DateTime CreatedOn { get; set; }

    [Required]
    public string OrderNumber { get; set; }

    [RequireNonDefault]
    public decimal Cost { get; set; }

    [RequireNonDefault]
    public PaymentMethod CostMethod { get; set; }

    public decimal? Prepayment { get; set; }

    public PaymentMethod? PrepaymentMethod { get; set; }

    public decimal? DeliveryCost { get; set; }

    public PaymentMethod? DeliveryCostMethod { get; set; }

    public int? Tax { get; set; }

    [RequireNonDefault]
    public decimal Profit { get; set; }

    [Required]
    public string Region { get; set; }

    [Required]
    public string City { get; set; }

    [Required]
    public string CompanyName { get; set; }

    [Required]
    public string ContactPerson { get; set; }

    [Required]
    public string Phone { get; set; }

    public string Email { get; set; }

    public string Notes { get; set; }

    [Required]
    [MinLength(1)]
    public IEnumerable<CorporateOrderProductRequestModel> Products { get; set; }
}
