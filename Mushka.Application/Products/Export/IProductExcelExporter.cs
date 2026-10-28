using System.Collections.Generic;
using System.IO;
using Mushka.Domain.Entities;

namespace Mushka.Application.Products.Export;

public interface IProductExcelExporter
{
    Stream Export(string title, IEnumerable<Product> products);
}
