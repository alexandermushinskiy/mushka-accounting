using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Mushka.Application.Orders.Export;
using Mushka.Application.Orders.Queries.Export;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Xunit;

namespace Mushka.Application.Tests.Orders.Queries.Export;

public class ExportOrdersQueryHandlerTests
{
    [Fact]
    public async Task Handle_ExportsOrdersMatchingRequestedIds()
    {
        var selectedId = Guid.NewGuid();
        var otherId = Guid.NewGuid();
        var orders = new[]
        {
            new Order { Id = selectedId, Number = "O-1" },
            new Order { Id = otherId, Number = "O-2" }
        };
        IEnumerable<Order> exported = null;
        string exportedTitle = null;
        var repository = new Mock<IOrderRepository>();
        repository
            .Setup(mock => mock.GetForExportAsync(It.IsAny<Expression<Func<Order, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Expression<Func<Order, bool>> predicate, CancellationToken _) =>
                orders.Where(predicate.Compile()).ToList());
        var exporter = new Mock<IOrderExcelExporter>();
        exporter
            .Setup(mock => mock.Export(It.IsAny<string>(), It.IsAny<IEnumerable<Order>>()))
            .Callback<string, IEnumerable<Order>>((title, exportedOrders) =>
            {
                exportedTitle = title;
                exported = exportedOrders;
            })
            .Returns(Stream.Null);
        var storage = new Mock<IStorage>();
        storage.Setup(mock => mock.GetRepository<IOrderRepository>()).Returns(repository.Object);
        var handler = new ExportOrdersQueryHandler(storage.Object, exporter.Object);

        var actual = await handler.Handle(new ExportOrdersQuery("March", new[] { selectedId }), CancellationToken.None);

        actual.Name.Should().Be("mushka_export_orders.xlsx");
        actual.ContentType.Should().Be("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        actual.Content.Should().BeSameAs(Stream.Null);
        exportedTitle.Should().Be("March");
        exported.Should().ContainSingle(order => order.Id == selectedId);
    }
}
