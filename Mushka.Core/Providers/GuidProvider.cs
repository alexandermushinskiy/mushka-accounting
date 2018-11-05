using System;
using Mushka.Core.Extensibility.Providers;

namespace Mushka.Core.Providers
{
    internal class GuidProvider : IGuidProvider
    {
        public Guid NewGuid()
        {
            return Guid.NewGuid();
        }
    }
}