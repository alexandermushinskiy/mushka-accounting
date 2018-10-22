using System;
using Mushka.Accounting.Core.Extensibility.Providers;

namespace Mushka.Accounting.Core.Providers
{
    internal class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetUtcNow() => DateTime.UtcNow;

        public DateTime GetNow() => DateTime.Now;
    }
}