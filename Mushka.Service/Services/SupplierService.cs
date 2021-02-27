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
    internal class SupplierService : ServiceBase<Supplier>, ISupplierService
    {
        private readonly IStorage storage;
        private readonly ISupplierRepository supplierRepository;

        public SupplierService(
            IStorage storage,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.storage = storage;
            
            supplierRepository = storage.GetRepository<ISupplierRepository>();
        }

        public async Task<OperationResult<IEnumerable<Supplier>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Supplier> suppliers = (await supplierRepository.GetAllAsync(cancellationToken))
                .OrderBy(supp => supp.Name)
                .ToList();

            return OperationResult<IEnumerable<Supplier>>.FromResult(suppliers);
        }

        public async Task<OperationResult<Supplier>> GetByIdAsync(Guid supplierId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var supplier = await supplierRepository.GetByIdAsync(supplierId, cancellationToken);

            return supplier == null
                ? OperationResult<Supplier>.FromError(ValidationErrors.SupplierNotFound, ValidationStatusType.NotFound)
                : OperationResult<Supplier>.FromResult(supplier);
        }

        public async Task<OperationResult<Supplier>> AddAsync(Supplier supplier, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (await supplierRepository.IsExistAsync(supp => supp.Name == supplier.Name, cancellationToken))
            {
                return OperationResult<Supplier>.FromError(ValidationErrors.SupplierWithNameExist);
            }
            
            var addedSupplier = supplierRepository.Add(supplier);
            await storage.SaveAsync(cancellationToken);

            return OperationResult<Supplier>.FromResult(addedSupplier);
        }

        public async Task<OperationResult<Supplier>> UpdateAsync(Supplier supplier, CancellationToken cancellationToken = default(CancellationToken))
        {
            var supplierToUpdate = await supplierRepository.GetByIdAsync(supplier.Id, cancellationToken);

            if (supplierToUpdate == null)
            {
                return OperationResult<Supplier>.FromError(ValidationErrors.SupplierNotFound, ValidationStatusType.NotFound);
            }

            if (await supplierRepository.IsExistAsync(supp => supp.Id != supplier.Id && supp.Name == supplier.Name, cancellationToken))
            {
                return OperationResult<Supplier>.FromError(ValidationErrors.SupplierWithNameExist);
            }

            supplier.CreatedOn = supplierToUpdate.CreatedOn;
            var updatedSupplier = supplierRepository.Update(supplier);
            await storage.SaveAsync(cancellationToken);

            return OperationResult<Supplier>.FromResult(updatedSupplier);
        }

        public async Task<OperationResult<Supplier>> DeleteAsync(Guid supplierId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var supplier = await supplierRepository.GetByIdAsync(supplierId, cancellationToken);

            if (supplier == null)
            {
                OperationResult<Supplier>.FromError(ValidationErrors.SupplierNotFound, ValidationStatusType.NotFound);
            }

            supplierRepository.Delete(supplier);
            await storage.SaveAsync(cancellationToken);

            return OperationResult<Supplier>.FromResult(supplier);
        }
    }
}