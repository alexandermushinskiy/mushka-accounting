using System;
using System.IO;
using System.Reflection;

namespace Mushka.Infrastructure.Excel.Services
{
    internal abstract class ExcelServiceBase
    {
        protected const int StartRowIndex = 8;
        protected const string CurrencyFormat = "### ### ##0.00";
        protected static readonly Guid SocksCategoryId = Guid.Parse("88CD0F34-9D4A-4E45-BE97-8899A97FB82C");

        protected static Stream GetTemplate(string templateName) =>
            Assembly.GetExecutingAssembly().GetManifestResourceStream(templateName);
    }
}