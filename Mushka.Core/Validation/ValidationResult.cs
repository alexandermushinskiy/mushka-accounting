using System.Collections.Generic;
using System.Linq;
using Mushka.Core.Extensibility.Validation;
using Mushka.Core.Validation.Enums;

namespace Mushka.Core.Validation
{
    public class ValidationResult : IValidationResult
    {
        private ValidationResult(IEnumerable<FieldError> errors, ValidationStatusType status)
        {
            Errors = errors.ToList();
            Status = status;
        }

        private ValidationResult(string message, ValidationStatusType status)
        {
            Message = message;
            Status = status;
        }

        public string Message { get; }

        public IEnumerable<FieldError> Errors { get; }

        public ValidationStatusType Status { get; }

        public static IValidationResult CreateInfo(string message, ValidationStatusType status = ValidationStatusType.Success)
            => new ValidationResult(message, status);

        public static IValidationResult CreateError(string errorKey, ValidationStatusType status = ValidationStatusType.Error)
            => new ValidationResult(errorKey, status);

        public bool IsValid()
        {
            return Status == ValidationStatusType.Success;
        }
    }
}