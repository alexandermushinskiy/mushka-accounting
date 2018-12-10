using Mushka.Core.Extensibility.Validation;
using Mushka.Core.Validation.Enums;

namespace Mushka.Core.Validation
{
    public class ValidationResult : IValidationResult
    {
        private ValidationResult(LevelType level, string message, ValidationStatusType status)
        {
            Level = level;
            Message = message;
            Status = status;
        }

        private ValidationResult(LevelType level, string message, string code, ValidationStatusType status)
            : this(level, message, status)
        {
            Code = code;
        }

        public LevelType Level { get; }

        public string Message { get; }

        public string Code { get; }

        public ValidationStatusType Status { get; }

        public static IValidationResult CreateInfo(string message, ValidationStatusType status = ValidationStatusType.Success)
            => new ValidationResult(LevelType.Info, message, status);

        public static IValidationResult CreateError(string message, ValidationStatusType status = ValidationStatusType.Error)
            => new ValidationResult(LevelType.Error, message, status);

        public static IValidationResult CreateWarning(string code, string message, ValidationStatusType status = ValidationStatusType.BadOperation)
            => new ValidationResult(LevelType.Warning, message, code, status);

        public bool IsValid()
        {
            return Level != LevelType.Error && Level != LevelType.Warning;
        }
    }
}