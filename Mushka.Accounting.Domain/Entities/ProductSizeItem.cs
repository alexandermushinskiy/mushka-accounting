using System;

namespace Mushka.Accounting.Domain.Entities
{
    public class ProductSizeItem
    {
        public Product Product { get; set; }
        public Guid Id { get; set; }
        public string Size { get; set; }
        public int Amount { get; set; }
    }
}