using System;
using AutoMapper;
using Mushka.Accounting.Core.Extensibility.Providers;
using Mushka.Accounting.Domain.Entities;
using Mushka.Accounting.WebApi.ClientModels.Product;

namespace Mushka.Accounting.WebApi.Resolvers
{
    public class DateTimeResolver : IValueResolver<ProductRequestModel, Product, DateTime>
    {
        private readonly IDateTimeProvider dateTimeProvider;

        public DateTimeResolver(IDateTimeProvider dateTimeProvider)
        {
            this.dateTimeProvider = dateTimeProvider;
        }

        public DateTime Resolve(
            ProductRequestModel source,
            Product destination,
            DateTime destMember,
            ResolutionContext context) => dateTimeProvider.GetNow();
    }
}