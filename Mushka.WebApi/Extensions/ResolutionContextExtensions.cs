using System;
using AutoMapper;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.Accounting.WebApi.Extensions
{
    public static class ResolutionContextExtensions
    {
        private const string IdName = nameof(IEntity.Id);

        public static Guid? GetId(this ResolutionContext context)
        {
            if (!context.Items.ContainsKey(IdName))
            {
                return null;
            }

            Guid.TryParse(context.Items[IdName].ToString(), out var id);
            return id;
        }
    }
}