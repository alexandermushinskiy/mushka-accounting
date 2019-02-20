using System;

namespace Mushka.Domain.Entities
{
    public class SetProduct
    {
        public Guid SetId { get; set; }
        public Set Set { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}