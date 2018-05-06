using System;
using System.Collections.Generic;
using System.Linq;
using Mushka.Accounting.Core.Extensibility.Validation;
using Mushka.Accounting.Core.Extensions;

namespace Mushka.Accounting.Core.Validation
{
    public class ValidationResponse : IValidationResponse
    {
        public ValidationResponse(IEnumerable<IValidationResult> validationResults)
        {
            ValidationResults = validationResults ?? Enumerable.Empty<IValidationResult>();
        }

        public IEnumerable<IValidationResult> ValidationResults { get; }

        public bool IsValid => ValidationResults.All(vr => vr.IsValid());
    }

    public class ValidationResponse<TResult> : ValidationResponse, IDisposable
    {
        public ValidationResponse(TResult result, IEnumerable<IValidationResult> validationResults)
            : base(validationResults)
        {
            Result = result;
        }

        public ValidationResponse(TResult result, IValidationResult validationResult)
            : this(result, validationResult.AsArray())
        {
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