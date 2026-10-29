using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.CorporateOrders.Converters;
using Mushka.Application.Exceptions;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;

namespace Mushka.Application.CorporateOrders.Commands;

public sealed class CorporateOrderCommandHandler :
    ICommandHandler<CreateCorporateOrderCommand>,
    ICommandHandler<UpdateCorporateOrderCommand>,
    ICommandHandler<DeleteCorporateOrderCommand>
{
    private readonly IStorage _storage;
    private readonly ICorporateOrderRepository _corporateOrderRepository;
    private readonly ICreateCorporateOrderConverter _createCorporateOrderConverter;
    private readonly IUpdateCorporateOrderConverter _updateCorporateOrderConverter;

    public CorporateOrderCommandHandler(
        IStorage storage,
        ICreateCorporateOrderConverter createCorporateOrderConverter,
        IUpdateCorporateOrderConverter updateCorporateOrderConverter)
    {
        _storage = storage;
        _corporateOrderRepository = storage.GetRepository<ICorporateOrderRepository>();
        _createCorporateOrderConverter = createCorporateOrderConverter;
        _updateCorporateOrderConverter = updateCorporateOrderConverter;
    }

    public async ValueTask<Unit> Handle(CreateCorporateOrderCommand command, CancellationToken cancellationToken)
    {
        _corporateOrderRepository.Add(_createCorporateOrderConverter.ToCorporateOrder(command));
        await _storage.SaveAsync(cancellationToken);

        return Unit.Value;
    }

    public async ValueTask<Unit> Handle(UpdateCorporateOrderCommand command, CancellationToken cancellationToken)
    {
        var corporateOrder = _updateCorporateOrderConverter.ToCorporateOrder(command);

        _corporateOrderRepository.DeleteProducts(corporateOrder.Id);
        _corporateOrderRepository.Update(corporateOrder);
        await _storage.SaveAsync(cancellationToken);

        return Unit.Value;
    }

    public async ValueTask<Unit> Handle(DeleteCorporateOrderCommand command, CancellationToken cancellationToken)
    {
        var corporateOrder = await _corporateOrderRepository.GetByIdAsync(command.Id, cancellationToken);

        if (corporateOrder == null)
        {
            throw new NotFoundException(ValidationErrors.CorporateOrderNotFound);
        }

        _corporateOrderRepository.Delete(corporateOrder);
        await _storage.SaveAsync(cancellationToken);

        return Unit.Value;
    }
}
