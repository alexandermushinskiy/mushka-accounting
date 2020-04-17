using System;
using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mushka.Core;
using Mushka.Infrastructure.DataAccess;
using Mushka.Infrastructure.DataAccess.Extensions;
using Mushka.Infrastructure.Excel;
using Mushka.Service;
using Mushka.WebApi.Filters;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using NLog.Web;

namespace Mushka.WebApi
{
    public class Startup
    {
        private const string CorsPolicyName = "CorsPolicy";
        public const string BearerAuthorizationHeaderKey = "Bearer";
        public const string ContentDispositionHeaderKey = "Content-Disposition";

        public Startup(IHostingEnvironment env)
        {
            Console.WriteLine("EnvironmentName: " + env.EnvironmentName);

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddMvcOptions(options => options.Filters.Add(typeof(ApiExceptionFilterAttribute)))
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.DateFormatString = "yyyy-MM-dd'T'HH:mm:ss'Z'";
                });

            services.AddDbContext(GetConnectionString());

            services.AddCors(options =>
            {
                options.AddPolicy(
                    CorsPolicyName,
                    builder => builder.AllowAnyOrigin()
                        .WithExposedHeaders(ContentDispositionHeaderKey)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddAutoMapper();
            Mapper.Configuration.AssertConfigurationIsValid();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(CorsPolicyName);

            loggerFactory.AddNLog();
            env.ConfigureNLog("Logging/nlog.config");

            app.UseMvc();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<CoreAutofacModule>();
            builder.RegisterModule<DataAccessAutofacModule>();
            builder.RegisterModule<ServiceAutofacModule>();
            builder.RegisterModule<ExcelAutofacModule>();
            builder.RegisterModule<WebApiAutofacModule>();
        }

        private string GetConnectionString()
        {
            return Configuration.GetConnectionString("MushkaDb");
        }

    }
}