﻿using Autofac;
using Mushka.Service.Extensibility.Services;
using Mushka.Service.Services;

namespace Mushka.Service
{
    public class ServiceAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CategoryService>().As<ICategoryService>();
            builder.RegisterType<ProductService>().As<IProductService>();
            builder.RegisterType<DeliveryService>().As<IDeliveryService>();
        }
    }
}