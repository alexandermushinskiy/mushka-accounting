using System;

namespace Mushka.Core.Extensibility.Providers
{
    public interface IDateTimeProvider
    {
        DateTime GetUtcNow();

        DateTime GetNow();
    }
}