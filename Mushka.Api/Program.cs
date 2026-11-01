using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mushka.Api.Extentions;
using Mushka.Api.Filters;
using Mushka.Application.Behaviors;
using Mushka.Application.Extentions;
using Mushka.Core.Extentions;
using Mushka.Infrastructure.DataAccess.Extentions;
using Newtonsoft.Json.Serialization;
using NLog.Web;

const string CorsPolicyName = "CorsPolicy";

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseNLog();

builder.Services
    .AddCore()
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddApi();

builder.Services
    .AddControllers()
    .AddMvcOptions(options => options.Filters.Add(typeof(ApiExceptionFilterAttribute)))
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        options.SerializerSettings.DateFormatString = "yyyy-MM-dd'T'HH:mm:ss'Z'";
    });

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
