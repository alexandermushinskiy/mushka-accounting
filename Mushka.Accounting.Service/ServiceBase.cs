using System.Collections.Generic;
using Mushka.Accounting.Core.Validation;
using Mushka.Accounting.Core.Validation.Enums;

namespace Mushka.Accounting.Service
{
    internal abstract class ServiceBase<TEntity> where TEntity : class
    {
        protected ValidationResponse<IEnumerable<TEntity>> CreateInfoValidationResponse(IEnumerable<TEntity> entities, string message) =>
            new ValidationResponse<IEnumerable<TEntity>>(entities, ValidationResult.CreateInfo(message));

        protected ValidationResponse<TEntity> CreateInfoValidationResponse(TEntity entity, string message) =>
            new ValidationResponse<TEntity>(entity, ValidationResult.CreateInfo(message));

        protected ValidationResponse<TEntity> CreateWarningValidationResponse(
            string message,
            ValidationStatusType validationStatus = ValidationStatusType.BadOperation) =>
            new ValidationResponse<TEntity>(null, ValidationResult.CreateWarning(message, validationStatus));
    }
}