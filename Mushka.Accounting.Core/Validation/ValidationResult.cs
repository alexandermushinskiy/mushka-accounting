using Mushka.Accounting.Core.Extensibility.Validation;
using Mushka.Accounting.Core.Validation.Enums;

namespace Mushka.Accounting.Core.Validation
{
    public class ValidationResult : IValidationResult
    {
        private ValidationResult(LevelType level, string message, ValidationStatusType status)
        {
            Level = level;
            Message = message;
            Status = status;
        }

        public LevelType Level { get; }

        public string Message { get; }

        public ValidationStatusType Status { get; }

        public static IValidationResult CreateInfo(string message, ValidationStatusType status = ValidationStatusType.Success)
            => new ValidationResult(LevelType.Info, message, status);

        public static IValidationResult CreateError(string message, ValidationStatusType status = ValidationStatusType.Error)
            => new ValidationResult(LevelType.Error, message, status);

        public static IValidationResult CreateWarning(string message, ValidationStatusType status = ValidationStatusType.BadOperation)
            => new ValidationResult(LevelType.Warning, message, status);

        public bool IsValid()
        {
            return Level != LevelType.Error && Level != LevelType.Warning;
        }
    }
}