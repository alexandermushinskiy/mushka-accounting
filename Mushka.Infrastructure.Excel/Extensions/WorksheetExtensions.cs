using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Mushka.Infrastructure.Excel.Extensions
{
    internal static class WorksheetExtensions
    {
        public static void SetTitle(this ExcelWorksheet worksheet, string title)
        {
            worksheet.Cells["B2"].Value = title;
            worksheet.Cells["B2"].Style.Font.Size = 14;
            worksheet.Cells["B2"].Style.Font.Bold = true;
        }

        public static void MergeRange(this ExcelWorksheet worksheet, int fromRow, int fromCol, int toRow, int toCol, string value)
        {
            using (var range = worksheet.Cells[fromRow, fromCol, toRow, toCol])
            {
                range.Merge = true;
                range.Value = value;
                range.Style.WrapText = true;
                range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
        }
    }
}