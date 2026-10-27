using System.Collections.Generic;
using System.IO;
using Mushka.Domain.Entities;

namespace Mushka.Application.Supplies.Export
{
    public interface ISupplyExcelExporter
    {
        Stream Export(IEnumerable<Supply> supplies, IEnumerable<Product> products);
    }
}
