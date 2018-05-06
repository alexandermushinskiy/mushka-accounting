using Mushka.Accounting.Core.Validation.Enums;

namespace Mushka.Accounting.Core.Extensibility.Validation
{
    public interface IValidationResult
    {
        LevelType Level { get; }

        string Message { get; }

        ValidationStatusType Status { get; }

        bool IsValid();
    }
}