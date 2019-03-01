using OfficeOpenXml;

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
    }
}