using System;

namespace Mushka.Accounting.Domain.Extensibility.Entities
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}