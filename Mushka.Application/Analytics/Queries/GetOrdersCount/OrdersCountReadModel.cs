using System;
using System.Collections.Generic;

namespace Mushka.Application.Analytics.Queries.GetOrdersCount;

public sealed class OrdersCountReadModel
{
    public IEnumerable<OrdersCountItemReadModel> Data { get; set; }
}

public sealed class OrdersCountItemReadModel
{
    public DateTime CreatedOn { get; set; }

    public int Quantity { get; set; }
}
