using System;

namespace Mushka.Domain.Dto
{
    public class OrdersCount
    {
        public DateTime CreatedOn { get; }

        public int Quantity { get; }

        public OrdersCount(DateTime createdOn, int quantity)
        {
            CreatedOn = createdOn;
            Quantity = quantity;
        }
    }
}