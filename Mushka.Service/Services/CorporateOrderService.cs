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

        public async Task<ValidationResponse<IEnumerable<CorporateOrder>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<CorporateOrder> orders = (await corporateOrderRepository.GetAllAsync(cancellationToken))
                .OrderBy(order => order.CreatedOn)
                .ToList();

            var message = orders.Any()
                ? "Corporate orders were successfully retrieved."
                : "No corporate orders found.";

            return CreateInfoValidationResponse(orders, message);
        }

        public async Task<ValidationResponse<CorporateOrder>> GetByIdAsync(Guid corporateOrderId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var corporateOrder = await corporateOrderRepository.GetByIdAsync(corporateOrderId, cancellationToken);

            return corporateOrder == null
                ? CreateWarningValidationResponse($"Corporate order with id {corporateOrderId} is not found.", ValidationStatusType.NotFound)
                : CreateInfoValidationResponse(corporateOrder, $"Corporate order with id {corporateOrderId} was successfully retrieved.");
        }

        public async Task<ValidationResponse<CorporateOrder>> AddAsync(CorporateOrder corporateOrder, CancellationToken cancellationToken = default(CancellationToken))
        {
            var addedCorporateOrder = corporateOrderRepository.Add(corporateOrder);
            await storage.SaveAsync(cancellationToken);

            return CreateInfoValidationResponse(addedCorporateOrder, $"Corporate order with id {addedCorporateOrder.Id} was successfully added.");
        }

        public async Task<ValidationResponse<CorporateOrder>> UpdateAsync(CorporateOrder corporateOrder, CancellationToken cancellationToken = default(CancellationToken))
        {
            corporateOrderRepository.DeleteProducts(corporateOrder.Id);

            var updatedCorporateOrder = corporateOrderRepository.Update(corporateOrder);
            await storage.SaveAsync(cancellationToken);

            return CreateInfoValidationResponse(updatedCorporateOrder, $"Corporate order with id {corporateOrder.Id} was successfully updated.");
        }

        public async Task<ValidationResponse<CorporateOrder>> DeleteAsync(Guid corporateOrderId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var corporateOrder = await corporateOrderRepository.GetByIdAsync(corporateOrderId, cancellationToken);

            if (corporateOrder == null)
            {
                return CreateWarningValidationResponse($"Corporate order with id {corporateOrderId} is not found.", ValidationStatusType.NotFound);
            }
            
            corporateOrderRepository.Delete(corporateOrder);
            await storage.SaveAsync(cancellationToken);

            return CreateInfoValidationResponse(corporateOrder, $"Corporate order with id {corporateOrder.Id} was successfully deleted.");
        }

        public async Task<ValidationResponse<bool>> IsNumberExistAsync(string orderNumber, CancellationToken cancellationToken = default(CancellationToken))
        {
            var isValid = !await corporateOrderRepository.IsExistAsync(order => order.Number == orderNumber, cancellationToken);

            return CreateInfoValidationResponse(isValid, $"Corporate order number {orderNumber} is {(isValid ? "" : "not ")}valid.");
        }
    }
}