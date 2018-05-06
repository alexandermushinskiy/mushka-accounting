using System.Collections.Generic;

namespace Mushka.Accounting.Core.Extensibility.Validation
{
    public interface IValidationResponse
    {
        bool IsValid { get; }

        IEnumerable<IValidationResult> ValidationResults { get; }
    }
}