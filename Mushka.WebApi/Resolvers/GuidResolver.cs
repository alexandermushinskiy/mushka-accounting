using System;
using AutoMapper;
using Mushka.Accounting.WebApi.Extensions;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.WebApi.Resolvers
{
    public class GuidResolver : IValueResolver<object, IEntity, Guid>
    {
        private readonly IGuidProvider guidProvider;

        public GuidResolver(IGuidProvider guidProvider)
        {
            this.guidProvider = guidProvider;
        }

        public Guid Resolve(object source, IEntity destination, Guid member, ResolutionContext context) =>
            context.GetId() ?? guidProvider.NewGuid();
    }
}