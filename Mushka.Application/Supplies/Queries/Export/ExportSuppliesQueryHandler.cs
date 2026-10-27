using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Supplies.Export;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;

namespace Mushka.Application.Supplies.Queries.Export
{
    public sealed class ExportSuppliesQueryHandler : IQueryHandler<ExportSuppliesQuery, ExportSuppliesReadModel>
    {
        private const string ExportContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private const string ExportFileName = "mushka_export_supply_products.xlsx";

        private readonly ISupplyRepository _supplyRepository;
        private readonly IProductRepository _productRepository;
        private readonly ISupplyExcelExporter _supplyExcelExporter;

        public ExportSuppliesQueryHandler(IStorage storage, ISupplyExcelExporter supplyExcelExporter)
        {
            _supplyRepository = storage.GetRepository<ISupplyRepository>();
            _productRepository = storage.GetRepository<IProductRepository>();
            _supplyExcelExporter = supplyExcelExporter;
        }

        public async ValueTask<ExportSuppliesReadModel> Handle(ExportSuppliesQuery query, CancellationToken cancellationToken)
        {
            var includes = new[]
            {
                nameof(Supply.Supplier),
                nameof(Supply.Products)
            };

            var supplies = (await _supplyRepository.GetAsync(
                    supply => query.SupplyIds.Contains(supply.Id),
                    includes,
                    cancellationToken))
                .OrderBy(supply => supply.Supplier.Name)
                .ThenBy(supply => supply.RequestDate)
                .ToList();

            var productIds = query.ProductIds.ToList();
            if (productIds.Count == 0)
            {
                productIds = supplies
                    .SelectMany(supply => supply.Products)
                    .Select(supplyProduct => supplyProduct.ProductId)
                    .ToList();
            }

            var products = (productIds.Count == 0
                    ? await _productRepository.GetAllAsync(cancellationToken)
                    : await _productRepository.GetAsync(product => productIds.Contains(product.Id), cancellationToken))
                .OrderBy(product => product.Name)
                .ThenBy(product => product.VendorCode)
                .ToList();

            return new ExportSuppliesReadModel
            {
                Name = ExportFileName,
                ContentType = ExportContentType,
                Content = _supplyExcelExporter.Export(supplies, products)
            };
        }
    }
}
