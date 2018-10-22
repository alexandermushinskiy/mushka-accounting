using System;

namespace Mushka.Accounting.Core.Extensibility.Providers
{
    public interface IDateTimeProvider
    {
        DateTime GetUtcNow();

        DateTime GetNow();
    }
}