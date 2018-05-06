using System;
using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mushka.Accounting.Core;
using Mushka.Accounting.Core.Extensibility.Http;
using Mushka.Accounting.Infrastructure.DataAccess;
using Mushka.Accounting.Infrastructure.DataAccess.Extensions;
using Mushka.Accounting.Service;
using Mushka.Accounting.WebApi.Filters;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using NLog.Web;

namespace Mushka.Accounting.WebApi
{
    public class Startup
    {
        private const string CorsPolicyName = "CorsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
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
                        .WithExposedHeaders(HttpConstants.ContentDispositionHeaderKey)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddAutoMapper(typeof(AccountingAutoMapperProfile));
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
            builder.RegisterModule<WebApiAutofacModule>();
        }

        private string GetConnectionString()
        {
            string sqlConnectionString = Configuration.GetConnectionString("AccountingDb");
            string user = Configuration.GetConnectionString("User");
            string password = Configuration.GetConnectionString("Password");
            string host = Configuration.GetConnectionString("Host");
            string database = Configuration.GetConnectionString("Database");

            return String.Format(sqlConnectionString, user, password, host, database);
        }

    }
}