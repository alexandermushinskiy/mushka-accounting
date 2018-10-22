namespace Mushka.Accounting.Core.Extensibility.Validation
{
    public interface IValidationResponse
    {
        bool IsValid { get; }

        IValidationResult ValidationResult { get; }
    }
}