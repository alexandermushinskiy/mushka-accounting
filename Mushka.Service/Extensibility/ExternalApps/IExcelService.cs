using System.Collections.Generic;
using System.IO;
using Mushka.Domain.Entities;

namespace Mushka.Service.Extensibility.ExternalApps
{
    public interface IExcelService
    {
        Stream ExportOrders(string title, IEnumerable<Order> orders);

        Stream ExportProducts(string title, IEnumerable<Product> products);
    }
}