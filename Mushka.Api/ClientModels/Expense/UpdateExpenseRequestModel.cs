using System;
using System.ComponentModel.DataAnnotations;
using Mushka.Api.Filters;
using Mushka.Domain.Entities;

namespace Mushka.Api.ClientModels.Expense;

public class UpdateExpenseRequestModel
{
    [RequireNonDefault]
    public DateTime CreatedOn { get; set; }

    [RequireNonDefault]
    public ExpenseCategory Category { get; set; }

    [Required]
    public string Purpose { get; set; }

    [RequireNonDefault]
    public decimal Cost { get; set; }

    [RequireNonDefault]
    public PaymentMethod CostMethod { get; set; }

    [Required]
    public string SupplierName { get; set; }

    public string Notes { get; set; }
}
