using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Mushka.Domain.Entities;
using Mushka.Infrastructure.Excel.Extensions;
using Mushka.Service.Extensibility.ExternalApps;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Mushka.Infrastructure.Excel.Services
{
    internal class SupplyExcelService : ExcelServiceBase, ISupplyExcelService
    {
        private const string ExportSupplyProductsTemplateName = "Mushka.Infrastructure.Excel.templates.export_supply_products_template.xlsx";

        public Stream ExportSupplies(IEnumerable<Supply> supplies, IEnumerable<Product> products)
        {
            var suppliesList = supplies.ToList();
            var productsList = products.ToList();

            var template = GetTemplate(ExportSupplyProductsTemplateName);

            using (var excelPackage = new ExcelPackage(template))
            {
                var worksheet = excelPackage.Workbook.Worksheets[0];

                var rowIndex = StartRowIndex;
                var productIndex = 0;
                foreach (var product in productsList)
                {
                    var productRowIndex = rowIndex;
                    var productSupplies = suppliesList.Where(sup => sup.Products.Any(prod => prod.ProductId == product.Id)).ToList();
                    var order = 1;
                    foreach (var supply in productSupplies)
                    {
                        var supplyProduct = supply.Products.Single(prod => prod.ProductId == product.Id);

                        worksheet.Cells[rowIndex, 3].Value = order;
                        worksheet.Cells[rowIndex, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[rowIndex, 3].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        worksheet.Cells[rowIndex, 4].Value = supply.Supplier.Name;
                        worksheet.Cells[rowIndex, 4].Style.WrapText = true;
                        worksheet.Cells[rowIndex, 4].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        worksheet.Cells[rowIndex, 5].Value = supply.ReceivedDate;
                        worksheet.Cells[rowIndex, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        worksheet.Cells[rowIndex, 5].Style.Numberformat.Format = "dd.mm.yyyy";
                        worksheet.Cells[rowIndex, 6].Value = supplyProduct.UnitPrice;
                        worksheet.Cells[rowIndex, 6].Style.Numberformat.Format = CurrencyFormat;
                        worksheet.Cells[rowIndex, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        worksheet.Cells[rowIndex, 7].Value = supplyProduct.Quantity;
                        worksheet.Cells[rowIndex, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        worksheet.Cells[rowIndex, 8].Value = supplyProduct.UnitPrice * supplyProduct.Quantity;
                        worksheet.Cells[rowIndex, 8].Style.Numberformat.Format = CurrencyFormat;
                        worksheet.Cells[rowIndex, 8].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

                        rowIndex++;
                        order++;
                    }

                    worksheet.MergeRange(productRowIndex, 1, rowIndex - 1, 1, product.Name + (product.Size != null ? $" ({product.Size.Name})" : ""));
                    worksheet.MergeRange(productRowIndex, 2, rowIndex - 1, 2, product.VendorCode);
                    
                    // set separator between products
                    using (var range = worksheet.Cells[rowIndex - 1, 1, rowIndex - 1, 8])
                    {
                        range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }

                    if (productsList.Count > 1 && productIndex % 2 == 0)
                    {
                        using (var range = worksheet.Cells[productRowIndex, 1, rowIndex - 1, 8])
                        {
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#DDEBF7"));
                        }
                    }

                    productIndex++;
                }

                return new MemoryStream(excelPackage.GetAsByteArray());
            }
        }
    }
}