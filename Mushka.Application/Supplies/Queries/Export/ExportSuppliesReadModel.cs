using System.IO;

namespace Mushka.Application.Supplies.Queries.Export
{
    public sealed class ExportSuppliesReadModel
    {
        public string Name { get; set; }

        public string ContentType { get; set; }

        public Stream Content { get; set; }
    }
}
