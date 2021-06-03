using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Extensibility.Logging;
using Mushka.Core.Validation;
using Mushka.Core.Validation.Enums;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;
using Mushka.Service.Extensibility.Services;

namespace Mushka.Service.Services
{
    internal class CorporateOrderService : ServiceBase<CorporateOrder>, ICorporateOrderService
    {
        private readonly IStorage storage;
        private readonly ICorporateOrderRepository corporateOrderRepository;

        public CorporateOrderService(
            IStorage storage,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.storage = storage;

            corporateOrderRepository = storage.GetRepository<ICorporateOrderRepository>();
        }

        public async Task<OperationResult<IEnumerable<CorporateOrder>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<CorporateOrder> orders = (await corporateOrderRepository.GetAllAsync(cancellationToken))
                .OrderBy(order => order.CreatedOn)
                .ToList();

            return OperationResult<IEnumerable<CorporateOrder>>.FromResult(orders);
        }

        public async Task<OperationResult<CorporateOrder>> GetByIdAsync(Guid corporateOrderId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var corporateOrder = await corporateOrderRepository.GetByIdAsync(corporateOrderId, cancellationToken);

            return corporateOrder == null
                ? OperationResult<CorporateOrder>.FromError(ValidationErrors.CorporateOrderNotFound, ValidationStatusType.NotFound)
                : OperationResult<CorporateOrder>.FromResult(corporateOrder);
        }

        public async Task<OperationResult> AddAsync(CorporateOrder corporateOrder, CancellationToken cancellationToken = default(CancellationToken))
        {
            corporateOrderRepository.Add(corporateOrder);
            await storage.SaveAsync(cancellationToken);

            return OperationResult.Success();
        }

        public async Task<OperationResult> UpdateAsync(CorporateOrder corporateOrder, CancellationToken cancellationToken = default(CancellationToken))
        {
            corporateOrderRepository.DeleteProducts(corporateOrder.Id);

            corporateOrderRepository.Update(corporateOrder);
            await storage.SaveAsync(cancellationToken);

            return OperationResult.Success();
        }

        public async Task<OperationResult> DeleteAsync(Guid corporateOrderId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var corporateOrder = await corporateOrderRepository.GetByIdAsync(corporateOrderId, cancellationToken);

            if (corporateOrder == null)
            {
                return OperationResult.FromError(ValidationErrors.CorporateOrderNotFound, ValidationStatusType.NotFound);
            }
            
            corporateOrderRepository.Delete(corporateOrder);
            await storage.SaveAsync(cancellationToken);

            return OperationResult.Success();
        }

        public async Task<OperationResult<bool>> IsNumberExistAsync(string orderNumber, CancellationToken cancellationToken = default(CancellationToken))
        {
            var isValid = !await corporateOrderRepository.IsExistAsync(order => order.Number == orderNumber, cancellationToken);

            return OperationResult<bool>.FromResult(isValid);
        }
    }
}