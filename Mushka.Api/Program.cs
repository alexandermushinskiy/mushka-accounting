using Autofac;
using Autofac.Extensions.DependencyInjection;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mushka.Api.Converters.Suppliers;
using Mushka.Api.Converters.Supplies;
using Mushka.Api.Exporters;
using Mushka.Api.Filters;
using Mushka.Application.Behaviors;
using Mushka.Application.Suppliers.Converters;
using Mushka.Application.Supplies.Converters;
using Mushka.Application.Supplies.Export;
using Mushka.Core;
using Mushka.Infrastructure.DataAccess;
using Mushka.Infrastructure.DataAccess.Extensions;
using Mushka.Infrastructure.Excel;
using Newtonsoft.Json.Serialization;
using NLog.Web;

const string CorsPolicyName = "CorsPolicy";
const string ContentDispositionHeaderKey = "Content-Disposition";

var builder = WebApplication.CreateBuilder(args);

builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .UseNLog()
    .ConfigureContainer<ContainerBuilder>(RegisterServices);

builder.Services
    .AddControllers()
    .AddMvcOptions(options => options.Filters.Add(typeof(ApiExceptionFilterAttribute)))
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        options.SerializerSettings.DateFormatString = "yyyy-MM-dd'T'HH:mm:ss'Z'";
    });

builder.Services.AddDbContext(builder.Configuration.GetConnectionString("MushkaDb"));

builder.Services.AddMediator(options =>
{
    options.ServiceLifetime = ServiceLifetime.Scoped;
    options.Namespace = "Mushka.Api.Mediator";
    options.PipelineBehaviors = [typeof(LoggingBehavior<,>)];
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        CorsPolicyName,
        policy => policy.AllowAnyOrigin()
            .WithExposedHeaders(ContentDispositionHeaderKey)
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.CustomSchemaIds(type => type.FullName?.Replace("+", ".")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mushka API v1");
    c.RoutePrefix = string.Empty;
});

app.UseRouting();
app.UseCors(CorsPolicyName);
app.MapControllers();

app.Run();

static void RegisterServices(ContainerBuilder container)
{
    container.RegisterModule<CoreAutofacModule>();
    container.RegisterModule<DataAccessAutofacModule>();
    container.RegisterModule<ExcelAutofacModule>();

    container.RegisterType<CreateSupplierConverter>().As<ICreateSupplierConverter>();
    container.RegisterType<UpdateSupplierConverter>().As<IUpdateSupplierConverter>();
    container.RegisterType<SearchSuppliersConverter>().As<ISearchSuppliersConverter>();
    container.RegisterType<DescribeSupplierConverter>().As<IDescribeSupplierConverter>();
    container.RegisterType<SupplierRequestConverter>().As<ISupplierRequestConverter>();

    container.RegisterType<CreateSupplyConverter>().As<ICreateSupplyConverter>();
    container.RegisterType<UpdateSupplyConverter>().As<IUpdateSupplyConverter>();
    container.RegisterType<SearchSuppliesConverter>().As<ISearchSuppliesConverter>();
    container.RegisterType<DescribeSupplyConverter>().As<IDescribeSupplyConverter>();
    container.RegisterType<SupplyRequestConverter>().As<ISupplyRequestConverter>();
    container.RegisterType<SupplyExcelExporter>().As<ISupplyExcelExporter>();
}
