using System.Collections.Generic;
using System.IO;
using Mushka.Application.Products.Export;
using Mushka.Domain.Entities;
using Mushka.Service.Extensibility.ExternalApps;

namespace Mushka.Api.Exporters;

public sealed class ProductExcelExporter : IProductExcelExporter
{
    private readonly IExcelService _excelService;

    public ProductExcelExporter(IExcelService excelService)
    {
        _excelService = excelService;
    }

    public Stream Export(string title, IEnumerable<Product> products) =>
        _excelService.ExportProducts(title, products);
}
