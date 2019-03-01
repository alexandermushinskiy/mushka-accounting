using System;
using AutoMapper;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Extensibility.Entities;
using Mushka.WebApi.Extensions;

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