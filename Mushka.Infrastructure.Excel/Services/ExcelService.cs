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
    internal class ExcelService : ExcelServiceBase, IExcelService
    {
        private const string ExportOrdersTemplateName = "Mushka.Infrastructure.Excel.templates.export_orders_template.xlsx";
        private const string ExportProductsTemplateName = "Mushka.Infrastructure.Excel.templates.export_products_template.xlsx";

        public Stream ExportOrders(string title, IEnumerable<Order> orders)
        {
            var ordersList = orders.ToList();
            var template = GetTemplate(ExportOrdersTemplateName);

            using (var excelPackage = new ExcelPackage(template))
            {
                var worksheet = excelPackage.Workbook.Worksheets["Orders"];

                var ordersFrom = ordersList.Min(ord => ord.OrderDate).ToString("dd.MM.yyyy");
                var ordersTo = ordersList.Max(ord => ord.OrderDate).ToString("dd.MM.yyyy");

                // set title
                worksheet.SetTitle($"Список проданных товаров за период с {ordersFrom} по {ordersTo}");

                var orderProducts = ordersList
                    .SelectMany(order => order.Products)
                    .Where(op => op.Product.CategoryId == SocksCategoryId)
                    .GroupBy(op => op.Product)
                    .Select(g => new { Product = g.Key, Quantity = g.Sum(x => x.Quantity), Cost = g.Sum(x => x.UnitPrice * x.Quantity) })
                    .OrderBy(gr => gr.Product.Name)
                    .ToList();

                //worksheet.Cells[1, 2].Formula = $"SUM(A1:A2)";
                //worksheet.Cells[1, 2].Calculate(new ExcelCalculationOption { AllowCirculareReferences = true });
                //worksheet.Cells[2, 2].Value = $"({worksheet.Cells[1, 1].Address}+{worksheet.Cells[1, 1].Value}:{worksheet.Cells[1, 2].Address}+{worksheet.Cells[2, 1].Value})";

                worksheet.Cells["C4"].Value = ordersList.Count;
                worksheet.Cells["C4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells["C5"].Value = orderProducts.Count;
                worksheet.Cells["C5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                var rowIndex = StartRowIndex;
                var total = 0;
                var totalCost = 0M;
                foreach (var orderProduct in orderProducts)
                {
                    worksheet.Cells[rowIndex, 1].Value = rowIndex - StartRowIndex + 1;
                    worksheet.Cells[rowIndex, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[rowIndex, 2].Value = orderProduct.Product.Name;
                    worksheet.Cells[rowIndex, 3].Value = orderProduct.Product.VendorCode;
                    worksheet.Cells[rowIndex, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[rowIndex, 4].Value = orderProduct.Product.Size.Name;
                    worksheet.Cells[rowIndex, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[rowIndex, 5].Value = orderProduct.Quantity;
                    worksheet.Cells[rowIndex, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[rowIndex, 6].Value = orderProduct.Cost;
                    worksheet.Cells[rowIndex, 6].Style.Numberformat.Format = CurrencyFormat;
                    
                    total += orderProduct.Quantity;
                    totalCost += orderProduct.Cost;
                    rowIndex++;
                }
                
                rowIndex += 1;
                worksheet.Cells[rowIndex, 4].Value = "ВСЕГО";
                worksheet.Cells[rowIndex, 4].Style.Font.Bold = true;
                worksheet.Cells[rowIndex, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                worksheet.Cells[rowIndex, 5].Value = total;
                worksheet.Cells[rowIndex, 5].Style.Font.Bold = true;
                worksheet.Cells[rowIndex, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                worksheet.Cells[rowIndex, 6].Value = totalCost;
                worksheet.Cells[rowIndex, 6].Style.Font.Bold = true;
                worksheet.Cells[rowIndex, 6].Style.Numberformat.Format = CurrencyFormat;

                using (ExcelRange range = worksheet.Cells[$"D{rowIndex}:F{rowIndex}"])
                {
                    range.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    range.Style.Font.Size = 12;
                }

                return new MemoryStream(excelPackage.GetAsByteArray());
            }
        }

        public Stream ExportProducts(string title, IEnumerable<Product> products)
        {
            var template = GetTemplate(ExportProductsTemplateName);

            using (var excelPackage = new ExcelPackage(template))
            {
                var worksheet = excelPackage.Workbook.Worksheets[0];
                worksheet.SetTitle("Список товаров - сверка остатка");
                
                var productsList = products
                    .Select(prod => new
                    {
                        prod.Name,
                        prod.VendorCode,
                        SizeName = prod.Size.Name,
                        Sold = prod.Orders.Sum(ord => ord.Quantity),
                        Received = prod.Supplies.Sum(sup => sup.Quantity),
                        Balance = prod.Quantity
                    })
                    .OrderBy(prod => prod.Name)
                    .ToList();

                worksheet.Cells["C4"].Value = productsList.Count;
                worksheet.Cells["C4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                var rowIndex = StartRowIndex;
                foreach (var product in productsList)
                {
                    worksheet.Cells[rowIndex, 1].Value = rowIndex - StartRowIndex + 1;
                    worksheet.Cells[rowIndex, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[rowIndex, 2].Value = product.Name;
                    worksheet.Cells[rowIndex, 3].Value = product.VendorCode;
                    worksheet.Cells[rowIndex, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[rowIndex, 4].Value = product.SizeName;
                    worksheet.Cells[rowIndex, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[rowIndex, 5].Value = product.Received;
                    worksheet.Cells[rowIndex, 6].Value = product.Sold;
                    worksheet.Cells[rowIndex, 7].Value = product.Balance;

                    if (product.Balance != product.Received - product.Sold)
                    {
                        worksheet.Cells[rowIndex, 8].Value = product.Received - product.Sold;
                        using (ExcelRange range = worksheet.Cells[rowIndex, 1, rowIndex, 7])
                        {
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.Red);
                        }
                    }

                    rowIndex++;
                }

                return new MemoryStream(excelPackage.GetAsByteArray());
            }
        }
    }
}