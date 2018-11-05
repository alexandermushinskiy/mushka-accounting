using System;
using AutoMapper;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Product;

namespace Mushka.WebApi.Resolvers
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