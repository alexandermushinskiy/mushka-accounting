using System;
using Mushka.WebApi.Extensibility.Configurations;

namespace Mushka.WebApi.Configurations
{
    internal class DatabaseSettingsProvider : IDatabaseSettingsProvider
    {
        public string GetConnectionString() => Environment.GetEnvironmentVariable(EnvironmentVariables.DatabaseConnection);
    }
}