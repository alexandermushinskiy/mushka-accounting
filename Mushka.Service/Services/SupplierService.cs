using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Extensibility.Logging;
using Mushka.Core.Validation;
using Mushka.Core.Validation.Codes;
using Mushka.Core.Validation.Enums;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Service.Extensibility.Services;

namespace Mushka.Service.Services
{
    internal class SupplierService : ServiceBase<Supplier>, ISupplierService
    {
        private readonly ISupplierRepository supplierRepository;

        public SupplierService(
            ISupplierRepository supplierRepository,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.supplierRepository = supplierRepository;
        }

        public async Task<ValidationResponse<IEnumerable<Supplier>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Supplier> suppliers = (await supplierRepository.GetAllAsync(cancellationToken))
                .OrderBy(supp => supp.Name)
                .ToList();

            string message = suppliers.Any()
                ? "Suppliers were successfully retrieved."
                : "No suppliers found.";

            return CreateInfoValidationResponse(suppliers, message);
        }

        public async Task<ValidationResponse<Supplier>> GetByIdAsync(Guid supplierId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Supplier supplier = await supplierRepository.GetByIdAsync(supplierId, cancellationToken);

            return supplier == null
                ? CreateWarningValidationResponse(ValidationCodes.SupplierNotFound, $"Supplier with id {supplierId} is not found.", ValidationStatusType.NotFound)
                : CreateInfoValidationResponse(supplier, $"Supplier with id {supplier.Id} was successfully retrieved.");
        }

        public async Task<ValidationResponse<Supplier>> AddAsync(Supplier supplier, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (await supplierRepository.IsExistAsync(supp => supp.Name == supplier.Name, cancellationToken))
            {
                return CreateWarningValidationResponse(ValidationCodes.SupplierNameExist, $"Supplier with name {supplier.Name} is already exist.");
            }
            
            Supplier addedSupplier = await supplierRepository.AddAsync(supplier, cancellationToken);

            return CreateInfoValidationResponse(addedSupplier, $"Supplier with id {supplier.Id} was successfully created.");
        }

        public async Task<ValidationResponse<Supplier>> UpdateAsync(Supplier supplier, CancellationToken cancellationToken = default(CancellationToken))
        {
            Supplier supplierToUpdate = await supplierRepository.GetByIdAsync(supplier.Id, cancellationToken);

            if (supplierToUpdate == null)
            {
                return CreateWarningValidationResponse(ValidationCodes.SupplierNotFound, $"Supplier with id {supplier.Id} is not found.", ValidationStatusType.NotFound);
            }

            if (await supplierRepository.IsExistAsync(supp => supp.Id != supplier.Id && supp.Name == supplier.Name, cancellationToken))
            {
                return CreateWarningValidationResponse(ValidationCodes.SupplierNameExist, $"Supplier with name {supplier.Name} is already exist.");
            }

            Supplier updatedSupplier = await supplierRepository.UpdateAsync(supplier, cancellationToken);

            return CreateInfoValidationResponse(updatedSupplier, $"Supplier with id {supplier.Id} was successfully updated.");
        }

        public async Task<ValidationResponse<Supplier>> DeleteAsync(Guid supplierId, CancellationToken cancellationToken = default(CancellationToken))
        {
            Supplier supplier = await supplierRepository.GetByIdAsync(supplierId, cancellationToken);

            if (supplier == null)
            {
                return CreateWarningValidationResponse(ValidationCodes.SupplierNotFound, $"Supplier with id {supplierId} is not found.", ValidationStatusType.NotFound);
            }

            await supplierRepository.DeleteAsync(supplier, cancellationToken);

            return CreateInfoValidationResponse(supplier, $"Supplier with id {supplier.Id} was successfully deleted.");
        }
    }
}