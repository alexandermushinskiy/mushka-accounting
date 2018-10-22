using System;
using Mushka.Accounting.Core.Extensibility.Providers;

namespace Mushka.Accounting.Core.Providers
{
    internal class GuidProvider : IGuidProvider
    {
        public Guid NewGuid()
        {
            return Guid.NewGuid();
        }
    }
}