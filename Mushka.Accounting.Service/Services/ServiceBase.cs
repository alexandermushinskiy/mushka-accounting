using System.Collections.Generic;
using Mushka.Accounting.Core.Extensibility.Logging;
using Mushka.Accounting.Core.Validation;
using Mushka.Accounting.Core.Validation.Enums;
using Mushka.Accounting.Domain.Extensibility.Entities;

namespace Mushka.Accounting.Service.Services
{
    internal abstract class ServiceBase<TEntity> : ServiceBase
        where TEntity : class, IEntity
    {
        protected ServiceBase(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        protected ValidationResponse<TEntity> GetAndLogWarningValidationResponse(
            string message,
            ValidationStatusType validationStatus = ValidationStatusType.BadOperation)
        {
            return GetAndLogWarningValidationResponse<TEntity>(message, validationStatus);
        }

        protected virtual ValidationResponse<IEnumerable<TEntity>> GetAndLogWarningValidationResponses(
            string message,
            ValidationStatusType validationStatus = ValidationStatusType.BadOperation)
        {
            return GetAndLogWarningValidationResponses<TEntity>(message, validationStatus);
        }

        protected ValidationResponse<TEntity> CreateInfoValidationResponse(TEntity entity, string message)
        {
            return CreateInfoValidationResponse<TEntity>(entity, message);
        }

        protected ValidationResponse<TEntity> CreateWarningValidationResponse(
            string message,
            ValidationStatusType validationStatus = ValidationStatusType.BadOperation)
        {
            return CreateWarningValidationResponse<TEntity>(message, validationStatus);
        }

        protected ValidationResponse<IEnumerable<TEntity>> CreateWarningValidationResponses(
            string message,
            ValidationStatusType validationStatus = ValidationStatusType.BadOperation)
        {
            return CreateWarningValidationResponses<TEntity>(message, validationStatus);
        }
    }

    internal abstract class ServiceBase
    {
        protected ILogger Logger { get; }

        protected ServiceBase(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType().Name);
        }

        protected virtual ValidationResponse<TResult> GetAndLogInfoValidationResponse<TResult>(TResult resultData, string message)
        {
            Logger.LogInfo(message);
            return new ValidationResponse<TResult>(resultData, ValidationResult.CreateInfo(message));
        }

        protected virtual ValidationResponse<IEnumerable<TResult>> GetAndLogInfoValidationResponses<TResult>(IEnumerable<TResult> results, string message)
        {
            Logger.LogInfo(message);
            return new ValidationResponse<IEnumerable<TResult>>(results, ValidationResult.CreateInfo(message));
        }

        protected virtual ValidationResponse<TResult> GetAndLogWarningValidationResponse<TResult>(
            string message,
            ValidationStatusType validationStatus = ValidationStatusType.BadOperation) where TResult : class
        {
            Logger.LogWarning(message);
            return CreateWarningValidationResponse<TResult>(message, validationStatus);
        }

        protected virtual ValidationResponse<IEnumerable<TResult>> GetAndLogWarningValidationResponses<TResult>(
            string message,
            ValidationStatusType validationStatus = ValidationStatusType.BadOperation)
        {
            Logger.LogWarning(message);
            return CreateWarningValidationResponses<TResult>(message, validationStatus);
        }

        protected ValidationResponse<TResult> CreateInfoValidationResponse<TResult>(TResult result, string message)
        {
            return new ValidationResponse<TResult>(result, ValidationResult.CreateInfo(message));
        }

        protected ValidationResponse<TResult> CreateWarningValidationResponse<TResult>(
            string message,
            ValidationStatusType validationStatus = ValidationStatusType.BadOperation) where TResult : class
        {
            return new ValidationResponse<TResult>(null, ValidationResult.CreateWarning(message, validationStatus));
        }

        protected ValidationResponse<IEnumerable<TResult>> CreateWarningValidationResponses<TResult>(
            string message,
            ValidationStatusType validationStatus = ValidationStatusType.BadOperation)
        {
            return new ValidationResponse<IEnumerable<TResult>>(null, ValidationResult.CreateWarning(message, validationStatus));
        }
    }
}