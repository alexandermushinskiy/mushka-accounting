using Mushka.Core.Extensibility.Logging;
using Mushka.Core.Validation;
using Mushka.Core.Validation.Enums;
using Mushka.Domain.Extensibility.Entities;

namespace Mushka.Service.Services
{
    internal abstract class ServiceBase<TEntity> : ServiceBase
        where TEntity : class, IEntity
    {
        protected ServiceBase(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        protected ValidationResponse<TEntity> CreateInfoValidationResponse(TEntity entity, string message)
        {
            return CreateInfoValidationResponse<TEntity>(entity, message);
        }

        protected ValidationResponse<TEntity> CreateErrorValidationResponse(
            string message,
            ValidationStatusType validationStatus = ValidationStatusType.BadOperation)
        {
            return CreateErrorValidationResponse<TEntity>(message, validationStatus);
        }
    }

    internal abstract class ServiceBase
    {
        protected ILogger Logger { get; }

        protected ServiceBase(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType().Name);
        }

        protected ValidationResponse<TResult> CreateInfoValidationResponse<TResult>(TResult result, string message)
        {
            return new ValidationResponse<TResult>(result, ValidationResult.CreateInfo(message));
        }

        protected ValidationResponse<TResult> CreateErrorValidationResponse<TResult>(
            string message,
            ValidationStatusType validationStatus = ValidationStatusType.BadOperation) where TResult : class
        {
            return new ValidationResponse<TResult>(null, ValidationResult.CreateError(message, validationStatus));
        }
    }
}