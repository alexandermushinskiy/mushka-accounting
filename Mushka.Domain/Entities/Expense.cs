using System;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.Domain.Entities
{
    public class Expense : IEntity
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public ExpenseCategory Category { get; set; }

        public string Purpose { get; set; }

        public decimal Cost { get; set; }

        public PaymentMethod CostMethod { get; set; }

        public string SupplierName { get; set; }

        public string Notes { get; set; }
    }
}