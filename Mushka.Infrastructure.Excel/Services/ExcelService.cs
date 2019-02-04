using System;
using System.Collections.Generic;
using System.IO;
using Mushka.Domain.Entities;
using Mushka.Service.Extensibility.ExternalApps;

namespace Mushka.Infrastructure.Excel.Services
{
    internal class ExcelService : IExcelService
    {
        public Stream ExportOrders(IEnumerable<Order> orders)
        {
            throw new NotImplementedException();
        }
    }
}