namespace Mushka.WebApi.Extensibility.Configurations
{
    public interface IDatabaseSettingsProvider
    {
        string GetConnectionString();
    }
}