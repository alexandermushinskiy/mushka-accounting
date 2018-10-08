using System;

namespace Mushka.Accounting.Domain.Entities
{
    public class Warehouse
    {
        public Guid Id { get; set; }
        public Product Product { get; set; }
        public int Amount { get; set; }
        public string Size { get; set; }
    }
}