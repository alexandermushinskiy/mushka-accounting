using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Products.Export;
using Mushka.Domain.Extensibility.Repositories;

namespace Mushka.Application.Products.Queries.Export;

public sealed class ExportProductsQueryHandler : IQueryHandler<ExportProductsQuery, ExportProductsReadModel>
{
    private const string ExportContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    private const string ExportFileName = "mushka_export_products.xlsx";

    private readonly IProductRepository _productRepository;
    private readonly IProductExcelExporter _productExcelExporter;

    public ExportProductsQueryHandler(IStorage storage, IProductExcelExporter productExcelExporter)
    {
        _productRepository = storage.GetRepository<IProductRepository>();
        _productExcelExporter = productExcelExporter;
    }

    public async ValueTask<ExportProductsReadModel> Handle(ExportProductsQuery query, CancellationToken cancellationToken)
    {
        var productIds = query.ProductIds.ToList();
        var products = await _productRepository.GetForExportAsync(
            product => productIds.Contains(product.Id),
            cancellationToken);

        return new ExportProductsReadModel
        {
            Name = ExportFileName,
            ContentType = ExportContentType,
            Content = _productExcelExporter.Export(query.Title, products)
        };
    }
}
