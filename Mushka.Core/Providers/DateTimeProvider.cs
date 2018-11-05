using System;
using Mushka.Core.Extensibility.Providers;

namespace Mushka.Core.Providers
{
    internal class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetUtcNow() => DateTime.UtcNow;

        public DateTime GetNow() => DateTime.Now;
    }
}