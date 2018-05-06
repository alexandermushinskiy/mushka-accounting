using System;

namespace Mushka.Accounting.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Sizes { get; set; }
        public bool IsSizesRequired { get; set; }
    }
}