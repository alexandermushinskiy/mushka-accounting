using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mushka.Domain.Entities;
using Mushka.Service.Extensibility.ExternalApps;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing;
using OfficeOpenXml.Style;

namespace Mushka.Infrastructure.Excel.Services
{
    internal class SupplyExcelService : ExcelServiceBase, ISupplyExcelService
    {
        private const string ExportSupplyProductsTemplateName = "Mushka.Infrastructure.Excel.templates.export_supply_products_template.xlsx";

        public Stream ExportSupplies(IEnumerable<Supply> supplies, IEnumerable<Product> products)
        {
            var suppliesList = supplies.ToList();

            var template = GetTemplate(ExportSupplyProductsTemplateName);

            using (var excelPackage = new ExcelPackage(template))
            {
                var worksheet = excelPackage.Workbook.Worksheets[0];

                var rowIndex = StartRowIndex;
                SetHeader(worksheet, rowIndex - 1);
                foreach (var product in products)
                {
                    var productIndex = rowIndex;
                    var order = 1;
                    foreach (var supply in suppliesList.Where(sup => sup.Products.Any(prod => prod.ProductId == product.Id)))
                    {
                        var supplyProduct = supply.Products.Single(prod => prod.ProductId == product.Id);

                        worksheet.Cells[rowIndex, 2].Value = order;
                        worksheet.Cells[rowIndex, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[rowIndex, 3].Value = supply.Supplier.Name;
                        worksheet.Cells[rowIndex, 4].Value = supply.ReceivedDate;
                        worksheet.Cells[rowIndex, 4].Style.Numberformat.Format = "dd.mm.yyyy";
                        worksheet.Cells[rowIndex, 5].Value = supplyProduct.UnitPrice;
                        worksheet.Cells[rowIndex, 5].Style.Numberformat.Format = CurrencyFormat;
                        worksheet.Cells[rowIndex, 6].Value = supplyProduct.Quantity;
                        worksheet.Cells[rowIndex, 7].Value = supplyProduct.UnitPrice * supplyProduct.Quantity;
                        worksheet.Cells[rowIndex, 7].Style.Numberformat.Format = CurrencyFormat;

                        rowIndex++;
                        order++;
                    }

                    //worksheet.Cells[rowIndex, 6].Formula = $"SUM(F{productIndex}:F{rowIndex})";
                    //worksheet.Cells[rowIndex, 6].Calculate(new ExcelCalculationOption { AllowCirculareReferences = true });
                    //worksheet.Cells[rowIndex, 7].Formula = $"SUM(G{productIndex}:G{rowIndex})";
                    //worksheet.Cells[rowIndex, 7].Calculate(new ExcelCalculationOption { AllowCirculareReferences = true });

                    //using ( var range = worksheet.Cells[rowIndex, 6, rowIndex, 7] )
                    //{
                    //    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    //}

                    using (var range = worksheet.Cells[productIndex, 1, rowIndex - 1, 1])
                    {
                        range.Merge = true;
                        range.Value = product.Name + "\r" + product.VendorCode + ( product.Size != null ? $"\r{product.Size.Name}" : "" );
                        range.Style.WrapText = true;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    using (var range = worksheet.Cells[rowIndex - 1, 1, rowIndex - 1, 7])
                    {
                        range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }

                    //rowIndex++;
                }

                return new MemoryStream(excelPackage.GetAsByteArray());
            }
        }

        private void SetHeader(ExcelWorksheet worksheet, int rowIndex)
        {
            worksheet.Cells[rowIndex, 1].Value = "Товар";
            worksheet.Cells[rowIndex, 2].Value = "№";
            worksheet.Cells[rowIndex, 3].Value = "Поставщик";
            worksheet.Cells[rowIndex, 4].Value = "Дата\r\nпоступления";
            worksheet.Cells[rowIndex, 4].Style.WrapText = true;
            worksheet.Cells[rowIndex, 5].Value = "Цена\r\nза единицу";
            worksheet.Cells[rowIndex, 5].Style.WrapText = true;
            worksheet.Cells[rowIndex, 6].Value = "Количество\r\nв поставке";
            worksheet.Cells[rowIndex, 6].Style.WrapText = true;
            worksheet.Cells[rowIndex, 7].Value = "На сумму";

            using (var range = worksheet.Cells[rowIndex, 1, rowIndex, 7])
            {
                range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Double;
                range.Style.Font.Size = 12;
            }
        }
    }
}