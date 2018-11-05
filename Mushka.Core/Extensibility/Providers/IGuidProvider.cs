using System;

namespace Mushka.Core.Extensibility.Providers
{
    public interface IGuidProvider
    {
        Guid NewGuid();
    }
}