using System.IO;

namespace Mushka.Application.Orders.Queries.Export;

public sealed class ExportOrdersReadModel
{
    public string Name { get; set; }

    public string ContentType { get; set; }

    public Stream Content { get; set; }
}
