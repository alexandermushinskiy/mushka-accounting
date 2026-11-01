using System.Collections.Generic;
using System.IO;
using Mushka.Application.Orders.Export;
using Mushka.Domain.Entities;
using Mushka.Service.Extensibility.ExternalApps;

namespace Mushka.Api.Exporters;

public sealed class OrderExcelExporter : IOrderExcelExporter
{
    private readonly IExcelService _excelService;

    public OrderExcelExporter(IExcelService excelService)
    {
        _excelService = excelService;
    }

    public Stream Export(string title, IEnumerable<Order> orders) =>
        _excelService.ExportOrders(title, orders);
}
