using System.Collections.Generic;
using Mushka.Core.Validation;
using Mushka.Core.Validation.Enums;

namespace Mushka.Core.Extensibility.Validation
{
    public interface IValidationResult
    {
        string Message { get; }

        ValidationStatusType Status { get; }

        IEnumerable<FieldError> Errors { get; }

        bool IsValid();
    }
}