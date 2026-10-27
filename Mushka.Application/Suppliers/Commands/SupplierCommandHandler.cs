using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Exceptions;
using Mushka.Application.Suppliers.Converters;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;

namespace Mushka.Application.Suppliers.Commands;

public sealed class SupplierCommandHandler :
    ICommandHandler<CreateSupplierCommand>,
    ICommandHandler<UpdateSupplierCommand>,
    ICommandHandler<DeleteSupplierCommand>
{
    private readonly IStorage _storage;
    private readonly ISupplierRepository _supplierRepository;
    private readonly ICreateSupplierConverter _createSupplierConverter;
    private readonly IUpdateSupplierConverter _updateSupplierConverter;

    public SupplierCommandHandler(
        IStorage storage,
        ICreateSupplierConverter createSupplierConverter,
        IUpdateSupplierConverter updateSupplierConverter)
    {
        _storage = storage;
        _supplierRepository = storage.GetRepository<ISupplierRepository>();
        _createSupplierConverter = createSupplierConverter;
        _updateSupplierConverter = updateSupplierConverter;
    }

    public async ValueTask<Unit> Handle(CreateSupplierCommand command, CancellationToken cancellationToken)
    {
        if (await _supplierRepository.IsExistAsync(supplier => supplier.Name == command.Supplier.Name, cancellationToken))
        {
            throw new BusinessRuleException(ValidationErrors.SupplierWithNameExist);
        }

        _supplierRepository.Add(_createSupplierConverter.ToSupplier(command));
        await _storage.SaveAsync(cancellationToken);

        return Unit.Value;
    }

    public async ValueTask<Unit> Handle(UpdateSupplierCommand command, CancellationToken cancellationToken)
    {
        var supplierToUpdate = await _supplierRepository.GetByIdAsync(command.Id, cancellationToken);

        if (supplierToUpdate == null)
        {
            throw new NotFoundException(ValidationErrors.SupplierNotFound);
        }

        if (await _supplierRepository.IsExistAsync(
                supplier => supplier.Id != command.Id && supplier.Name == command.Supplier.Name,
                cancellationToken))
        {
            throw new BusinessRuleException(ValidationErrors.SupplierWithNameExist);
        }

        var supplier = _updateSupplierConverter.ToSupplier(command);
        supplier.CreatedOn = supplierToUpdate.CreatedOn;
        _supplierRepository.Update(supplier);
        await _storage.SaveAsync(cancellationToken);

        return Unit.Value;
    }

    public async ValueTask<Unit> Handle(DeleteSupplierCommand command, CancellationToken cancellationToken)
    {
        var supplier = await _supplierRepository.GetByIdAsync(command.Id, cancellationToken);

        if (supplier == null)
        {
            throw new NotFoundException(ValidationErrors.SupplierNotFound);
        }

        _supplierRepository.Delete(supplier);
        await _storage.SaveAsync(cancellationToken);

        return Unit.Value;
    }
}
