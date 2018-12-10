using Moq;
using Mushka.Core.Extensibility.Logging;
using Mushka.Core.Validation;
using Mushka.Core.Validation.Enums;
using Mushka.Tests.Common;

namespace Mushka.Tests.Service
{
    public abstract class ServiceTestBase : UnitTestBase
    {
        protected readonly Mock<ILogger> LoggerMock;

        protected ServiceTestBase()
        {
            LoggerMock = MockRepository.Create<ILogger>();
        }

        protected static ValidationResponse<TEntity> CreateWarningValidationResponse<TEntity>(
            string code,
            string message,
            ValidationStatusType validationStatus = ValidationStatusType.BadOperation) =>
            new ValidationResponse<TEntity>(default(TEntity), ValidationResult.CreateWarning(code, message, validationStatus));

        protected static ValidationResponse<TEntity> CreateValidValidationResponse<TEntity>(
            TEntity source,
            string message,
            ValidationStatusType validationStatus = ValidationStatusType.Success) =>
            new ValidationResponse<TEntity>(source, ValidationResult.CreateInfo(message, validationStatus));

        protected static ValidationResponse<TEntity> CreateForbiddenValidationResponse<TEntity>(
            string code,
            string message,
            ValidationStatusType validationStatus = ValidationStatusType.Forbidden) =>
            new ValidationResponse<TEntity>(default(TEntity), ValidationResult.CreateWarning(code, message, validationStatus));
    }
}