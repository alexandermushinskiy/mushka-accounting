using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Exceptions;
using Mushka.Application.Exhibitions.Converters;
using Mushka.Domain.Comparers;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;

namespace Mushka.Application.Exhibitions.Commands;

public sealed class ExhibitionCommandHandler :
    ICommandHandler<CreateExhibitionCommand>,
    ICommandHandler<UpdateExhibitionCommand>,
    ICommandHandler<DeleteExhibitionCommand>
{
    private readonly IStorage _storage;
    private readonly IExhibitionRepository _exhibitionRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICreateExhibitionConverter _createExhibitionConverter;
    private readonly IUpdateExhibitionConverter _updateExhibitionConverter;

    public ExhibitionCommandHandler(
        IStorage storage,
        ICreateExhibitionConverter createExhibitionConverter,
        IUpdateExhibitionConverter updateExhibitionConverter)
    {
        _storage = storage;
        _exhibitionRepository = storage.GetRepository<IExhibitionRepository>();
        _productRepository = storage.GetRepository<IProductRepository>();
        _createExhibitionConverter = createExhibitionConverter;
        _updateExhibitionConverter = updateExhibitionConverter;
    }

    public async ValueTask<Unit> Handle(CreateExhibitionCommand command, CancellationToken cancellationToken)
    {
        var exhibition = _createExhibitionConverter.ToExhibition(command);

        foreach (var exhibitionProduct in exhibition.Products)
        {
            var storedProduct = await _productRepository.GetByIdAsync(exhibitionProduct.ProductId, cancellationToken);

            if (storedProduct == null)
            {
                throw new NotFoundException(ValidationErrors.ExhibitionNotFound);
            }

            if (storedProduct.Quantity < exhibitionProduct.Quantity)
            {
                throw new BusinessRuleException(ValidationErrors.ProductNotEnoughInStock);
            }

            storedProduct.Quantity -= exhibitionProduct.Quantity;
            _productRepository.Update(storedProduct);
        }

        _exhibitionRepository.Add(exhibition);
        await _storage.SaveAsync(cancellationToken);

        return Unit.Value;
    }

    public async ValueTask<Unit> Handle(UpdateExhibitionCommand command, CancellationToken cancellationToken)
    {
        var storedExhibition = await _exhibitionRepository.GetByIdAsync(command.Id, cancellationToken);

        if (storedExhibition == null)
        {
            throw new NotFoundException(ValidationErrors.ExhibitionNotFound);
        }

        var exhibition = _updateExhibitionConverter.ToExhibition(command);

        foreach (var exhibitionProduct in exhibition.Products)
        {
            var storedProduct = await _productRepository.GetByIdAsync(exhibitionProduct.ProductId, cancellationToken);

            if (storedProduct == null)
            {
                throw new NotFoundException(ValidationErrors.ProductNotFound);
            }

            var storedOrderQuantity = storedExhibition.Products
                .FirstOrDefault(product => product.ProductId == storedProduct.Id)?.Quantity ?? 0;

            if (storedOrderQuantity != exhibitionProduct.Quantity)
            {
                storedProduct.Quantity = storedProduct.Quantity + storedOrderQuantity - exhibitionProduct.Quantity;
                _productRepository.Update(storedProduct);
            }
        }

        foreach (var removedProduct in storedExhibition.Products.Except(exhibition.Products, new ExhibitionProductComparer()))
        {
            var storedProduct = await _productRepository.GetByIdAsync(removedProduct.ProductId, cancellationToken);

            if (storedProduct == null)
            {
                throw new NotFoundException(ValidationErrors.ProductNotFound);
            }

            storedProduct.Quantity += removedProduct.Quantity;
            _productRepository.Update(storedProduct);
        }

        _exhibitionRepository.Update(exhibition);
        await _storage.SaveAsync(cancellationToken);

        return Unit.Value;
    }

    public async ValueTask<Unit> Handle(DeleteExhibitionCommand command, CancellationToken cancellationToken)
    {
        var exhibition = await _exhibitionRepository.GetByIdAsync(command.Id, cancellationToken);

        if (exhibition == null)
        {
            throw new NotFoundException(ValidationErrors.ExhibitionNotFound);
        }

        foreach (var exhibitionProduct in exhibition.Products)
        {
            var storedProduct = await _productRepository.GetByIdAsync(exhibitionProduct.ProductId, cancellationToken);

            if (storedProduct == null)
            {
                throw new NotFoundException(ValidationErrors.ProductNotFound);
            }

            storedProduct.Quantity += exhibitionProduct.Quantity;
            _productRepository.Update(storedProduct);
        }

        _exhibitionRepository.Delete(exhibition);
        await _storage.SaveAsync(cancellationToken);

        return Unit.Value;
    }
}
