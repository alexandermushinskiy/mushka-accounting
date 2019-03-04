using System.Collections.Generic;
using System.IO;
using Mushka.Domain.Entities;

namespace Mushka.Service.Extensibility.ExternalApps
{
    public interface ISupplyExcelService
    {
        Stream ExportSupplies(IEnumerable<Supply> supplies, IEnumerable<Product> products);
    }
}