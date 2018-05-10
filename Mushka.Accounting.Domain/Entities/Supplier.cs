using System;
using Mushka.Accounting.Domain.Extensibility.Entities;

namespace Mushka.Accounting.Domain.Entities
{
    public class Supplier : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public string ContactPerson { get; set; }
        public string PaymentConditions { get; set; }
        public string Services { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}