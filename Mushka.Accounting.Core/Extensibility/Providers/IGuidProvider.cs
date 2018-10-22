using System;

namespace Mushka.Accounting.Core.Extensibility.Providers
{
    public interface IGuidProvider
    {
        Guid NewGuid();
    }
}