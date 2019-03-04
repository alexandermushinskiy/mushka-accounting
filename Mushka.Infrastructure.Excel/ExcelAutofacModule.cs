using Autofac;
using Mushka.Infrastructure.Excel.Services;
using Mushka.Service.Extensibility.ExternalApps;

namespace Mushka.Infrastructure.Excel
{
    public class ExcelAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ExcelService>().As<IExcelService>();
            builder.RegisterType<SupplyExcelService>().As<ISupplyExcelService>();
        }
    }
}