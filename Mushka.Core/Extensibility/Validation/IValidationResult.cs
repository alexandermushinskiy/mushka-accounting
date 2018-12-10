using Mushka.Core.Validation.Enums;

namespace Mushka.Core.Extensibility.Validation
{
    public interface IValidationResult
    {
        LevelType Level { get; }

        string Code { get; }

        string Message { get; }

        ValidationStatusType Status { get; }

        bool IsValid();
    }
}