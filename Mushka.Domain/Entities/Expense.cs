using System;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.Domain.Entities
{
    public class Expense : IEntity
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public ExpenseCategory Category { get; set; }

        public decimal Cost { get; set; }

        public PaymentMethod CostMethod { get; set; }

        public string Reason { get; set; }

        public string Notes { get; set; }
    }

    public enum ExpenseCategory
    {
        Advertising = 1,
        Equipment = 2,
        Photography = 3,
        Design = 4,
        WebSite = 5,
        Polygraphy = 6
    }
}