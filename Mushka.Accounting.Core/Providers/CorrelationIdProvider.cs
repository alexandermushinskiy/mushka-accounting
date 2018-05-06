using System;
using System.Security.Cryptography;
using Mushka.Accounting.Core.Extensibility.Providers;

namespace Mushka.Accounting.Core.Providers
{
    internal class CorrelationIdProvider : ICorrelationIdProvider
    {
        public string Generate()
        {
            byte[] bytes = new byte[8];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }

            return BitConverter.ToString(bytes).Replace("-", String.Empty).ToUpper();
        }
    }
}