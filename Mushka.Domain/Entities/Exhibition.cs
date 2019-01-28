using System;
using System.Collections.Generic;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.Domain.Entities
{
    public class Exhibition : IEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public string City { get; set; }

        public decimal ParticipationCost { get; set; }

        public string Notes { get; set; }

        public ICollection<OrderProduct> Products { get; set; }
    }
}