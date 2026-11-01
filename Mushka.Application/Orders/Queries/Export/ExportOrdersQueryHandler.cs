using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Orders.Export;
using Mushka.Domain.Extensibility.Repositories;

namespace Mushka.Application.Orders.Queries.Export;

public sealed class ExportOrdersQueryHandler : IQueryHandler<ExportOrdersQuery, ExportOrdersReadModel>
{
    private const string ExportContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    private const string ExportFileName = "mushka_export_orders.xlsx";

    private readonly IOrderRepository _orderRepository;
    private readonly IOrderExcelExporter _orderExcelExporter;

    public ExportOrdersQueryHandler(IStorage storage, IOrderExcelExporter orderExcelExporter)
    {
        _orderRepository = storage.GetRepository<IOrderRepository>();
        _orderExcelExporter = orderExcelExporter;
    }

    public async ValueTask<ExportOrdersReadModel> Handle(ExportOrdersQuery query, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetForExportAsync(order => query.OrderIds.Contains(order.Id), cancellationToken);

        return new ExportOrdersReadModel
        {
            Name = ExportFileName,
            ContentType = ExportContentType,
            Content = _orderExcelExporter.Export(query.Title, orders)
        };
    }
}
