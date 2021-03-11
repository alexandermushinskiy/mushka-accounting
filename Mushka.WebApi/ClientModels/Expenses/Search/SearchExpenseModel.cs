using System;
using Mushka.Domain.Entities;

namespace Mushka.WebApi.ClientModels.Expenses.Search
{
    public class SearchExpenseModel
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public ExpenseCategory Category { get; set; }

        public string Purpose { get; set; }

        public decimal Cost { get; set; }

        public string SupplierName { get; set; }
    }
}