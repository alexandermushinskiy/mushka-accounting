using System;
using System.Collections.Generic;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.Domain.Entities
{
    public class Exhibition : IEntity
    {
        public Exhibition()
        {
            Products = new List<ExhibitionProduct>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string City { get; set; }

        public decimal ParticipationCost { get; set; }
        
        public PaymentMethod ParticipationCostMethod { get; set; }

        public string Notes { get; set; }

        public decimal Profit { get; set; }

        public ICollection<ExhibitionProduct> Products { get; set; }
    }
}