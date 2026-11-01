using System.Collections.Generic;
using System.IO;
using Mushka.Domain.Entities;

namespace Mushka.Application.Orders.Export;

public interface IOrderExcelExporter
{
    Stream Export(string title, IEnumerable<Order> orders);
}
