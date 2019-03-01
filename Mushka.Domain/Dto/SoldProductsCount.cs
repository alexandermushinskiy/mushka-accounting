using System;

namespace Mushka.Domain.Dto
{
    public class SoldProductsCount
    {
        public DateTime CreatedOn { get; }

        public int Quantity { get; }

        public SoldProductsCount(DateTime createdOn, int quantity)
        {
            CreatedOn = createdOn;
            Quantity = quantity;
        }
    }
}