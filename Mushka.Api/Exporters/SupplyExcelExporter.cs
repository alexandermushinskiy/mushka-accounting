using System.Collections.Generic;
using System.IO;
using Mushka.Application.Supplies.Export;
using Mushka.Domain.Entities;
using Mushka.Service.Extensibility.ExternalApps;

namespace Mushka.Api.Exporters
{
    public sealed class SupplyExcelExporter : ISupplyExcelExporter
    {
        private readonly ISupplyExcelService _supplyExcelService;

        public SupplyExcelExporter(ISupplyExcelService supplyExcelService)
        {
            _supplyExcelService = supplyExcelService;
        }

        public Stream Export(IEnumerable<Supply> supplies, IEnumerable<Product> products) =>
            _supplyExcelService.ExportSupplies(supplies, products);
    }
}
