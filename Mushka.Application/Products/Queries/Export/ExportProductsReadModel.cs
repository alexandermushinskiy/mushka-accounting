using System.IO;

namespace Mushka.Application.Products.Queries.Export;

public sealed class ExportProductsReadModel
{
    public string Name { get; set; }

    public string ContentType { get; set; }

    public Stream Content { get; set; }
}
