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

        protected OperationResult<TEntity> CreateErrorOperationResult(
            string message,
            ValidationStatusType validationStatus = ValidationStatusType.BadOperation)
        {
            return CreateErrorOperationResult<TEntity>(message, validationStatus);
        }
    }

    internal abstract class ServiceBase
    {
        protected ILogger Logger { get; }

        protected ServiceBase(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType().Name);
        }

        protected OperationResult<TResult> CreateErrorOperationResult<TResult>(
            string message,
            ValidationStatusType validationStatus)
        {
            return OperationResult<TResult>.FromError(message, validationStatus);
        }
    }
}