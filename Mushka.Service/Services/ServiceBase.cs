using System.Collections.Generic;
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

        protected ValidationResponse<TEntity> GetAndLogWarningValidationResponse(
            string code,
            string message,
            ValidationStatusType validationStatus = ValidationStatusType.BadOperation)
        {
            return GetAndLogWarningValidationResponse<TEntity>(code, message, validationStatus);
        }

        protected virtual ValidationResponse<IEnumerable<TEntity>> GetAndLogWarningValidationResponses(
            string code,
            string message,
            ValidationStatusType validationStatus = ValidationStatusType.BadOperation)
        {
            return GetAndLogWarningValidationResponses<TEntity>(code, message, validationStatus);
        }

        protected ValidationResponse<TEntity> CreateInfoValidationResponse(TEntity entity, string message)
        {
            return CreateInfoValidationResponse<TEntity>(entity, message);
        }

        protected ValidationResponse<TEntity> CreateWarningValidationResponse(
            string code,
            string message,
            ValidationStatusType validationStatus = ValidationStatusType.BadOperation)
        {
            return CreateWarningValidationResponse<TEntity>(code, message, validationStatus);
        }

        protected ValidationResponse<IEnumerable<TEntity>> CreateWarningValidationResponses(
            string code,
            string message,
            ValidationStatusType validationStatus = ValidationStatusType.BadOperation)
        {
            return CreateWarningValidationResponses<TEntity>(code, message, validationStatus);
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
            string code,
            string message,
            ValidationStatusType validationStatus = ValidationStatusType.BadOperation) where TResult : class
        {
            Logger.LogWarning(message);
            return CreateWarningValidationResponse<TResult>(code, message, validationStatus);
        }

        protected virtual ValidationResponse<IEnumerable<TResult>> GetAndLogWarningValidationResponses<TResult>(
            string code,
            string message,
            ValidationStatusType validationStatus = ValidationStatusType.BadOperation)
        {
            Logger.LogWarning(message);
            return CreateWarningValidationResponses<TResult>(code, message, validationStatus);
        }

        protected ValidationResponse<TResult> CreateInfoValidationResponse<TResult>(TResult result, string message)
        {
            return new ValidationResponse<TResult>(result, ValidationResult.CreateInfo(message));
        }

        protected ValidationResponse<TResult> CreateWarningValidationResponse<TResult>(
            string code,
            string message,
            ValidationStatusType validationStatus = ValidationStatusType.BadOperation) where TResult : class
        {
            return new ValidationResponse<TResult>(null, ValidationResult.CreateWarning(code, message, validationStatus));
        }

        protected ValidationResponse<IEnumerable<TResult>> CreateWarningValidationResponses<TResult>(
            string code,
            string message,
            ValidationStatusType validationStatus = ValidationStatusType.BadOperation)
        {
            return new ValidationResponse<IEnumerable<TResult>>(null, ValidationResult.CreateWarning(code, message, validationStatus));
        }
    }
}