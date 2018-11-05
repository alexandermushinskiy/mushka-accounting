using System;

namespace Mushka.Domain.Extensibility.Entities
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}