using System;
using System.ComponentModel.DataAnnotations;
using Mushka.Domain.Entities;
using Mushka.WebApi.Filters;

namespace Mushka.WebApi.ClientModels.Expenses
{
    public class ExpenseRequestModel
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
}