using System.Collections.Generic;
using System.Linq;
using Mushka.Core.Validation.Enums;

namespace Mushka.Core.Validation
{
    public class OperationResult
    {
        public ValidationStatusType Status { get; }

        public IReadOnlyList<FieldError> Errors { get; }

        public bool IsSuccess => Status == ValidationStatusType.Success;

        public bool IsFailure => Status != ValidationStatusType.Success;

        protected OperationResult(ValidationStatusType status)
        {
            Status = status;
            Errors = new List<FieldError>();
        }

        protected OperationResult(IEnumerable<FieldError> errors, ValidationStatusType status)
        {
            Errors = errors.ToList();
            Status = status;
        }
    }

    public class OperationResult<TResultData> : OperationResult
    {
        public TResultData Data { get; }

        private OperationResult(TResultData data)
            : base(ValidationStatusType.Success)
        {
            Data = data;
        }

        private OperationResult(IEnumerable<FieldError> errors, ValidationStatusType status)
            : base(errors, status)
        {
        }

        public static OperationResult<TResultData> FromResult(TResultData data)
            => new OperationResult<TResultData>(data);

        public static OperationResult<TResultData> FromError(string errorKey, ValidationStatusType status = ValidationStatusType.BadOperation)
            => new OperationResult<TResultData>(new[] { new FieldError(errorKey) }, status);

        public static OperationResult<TResultData> FromErrors(IEnumerable<FieldError> errors, ValidationStatusType status = ValidationStatusType.BadOperation)
            => new OperationResult<TResultData>(errors, status);
    }
}