using System;
using AutoMapper;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.WebApi.Extensions
{
    public static class ResolutionContextExtensions
    {
        private const string IdName = nameof(IEntity.Id);

        public static Guid? GetId(this ResolutionContext context)
        {
            if (!context.TryGetItems(out var items) || !items.TryGetValue(IdName, out var value))
            {
                return null;
            }

            return Guid.TryParse(value?.ToString(), out var id) ? id : (Guid?)null;
        }
    }
}