using System;
using Mushka.Accounting.Core.Extensibility.Validation;

namespace Mushka.Accounting.Core.Validation
{
    public class ValidationResponse : IValidationResponse
    {
        public ValidationResponse(IValidationResult validationResult)
        {
            ValidationResult = validationResult;
        }

        public IValidationResult ValidationResult { get; }

        public bool IsValid => ValidationResult.IsValid();
    }

    public class ValidationResponse<TResult> : ValidationResponse, IDisposable
    {
        public ValidationResponse(TResult result, IValidationResult validationResult)
            : base(validationResult)
        {
            Result = result;
        }

        public TResult Result { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Result is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}