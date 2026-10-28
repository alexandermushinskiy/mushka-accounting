using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Exceptions;
using Mushka.Application.Products.Converters;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;

namespace Mushka.Application.Products.Commands;

public sealed class ProductCommandHandler :
    ICommandHandler<CreateProductCommand>,
    ICommandHandler<UpdateProductCommand>,
    ICommandHandler<DeleteProductCommand>
{
    private readonly IStorage _storage;
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICreateProductConverter _createProductConverter;
    private readonly IUpdateProductConverter _updateProductConverter;

    public ProductCommandHandler(
        IStorage storage,
        ICreateProductConverter createProductConverter,
        IUpdateProductConverter updateProductConverter)
    {
        _storage = storage;
        _productRepository = storage.GetRepository<IProductRepository>();
        _categoryRepository = storage.GetRepository<ICategoryRepository>();
        _createProductConverter = createProductConverter;
        _updateProductConverter = updateProductConverter;
    }

    public async ValueTask<Unit> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        if (!await _categoryRepository.IsExistAsync(category => category.Id == command.Product.CategoryId, cancellationToken))
        {
            throw new NotFoundException(ValidationErrors.CategoryNotFound);
        }

        if (await _productRepository.IsExistAsync(product => product.VendorCode == command.Product.VendorCode, cancellationToken))
        {
            throw new BusinessRuleException(ValidationErrors.ProductWithVendorCodeExist);
        }

        _productRepository.Add(_createProductConverter.ToProduct(command));
        await _storage.SaveAsync(cancellationToken);

        return Unit.Value;
    }

    public async ValueTask<Unit> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var productToUpdate = await _productRepository.GetByIdAsync(command.Id, cancellationToken);

        if (productToUpdate == null)
        {
            throw new NotFoundException(ValidationErrors.ProductNotFound);
        }

        if (await _productRepository.IsExistAsync(
                product => product.Id != command.Id && product.VendorCode == command.Product.VendorCode,
                cancellationToken))
        {
            throw new BusinessRuleException(ValidationErrors.ProductWithVendorCodeExist);
        }

        var product = _updateProductConverter.ToProduct(command);
        product.Quantity = productToUpdate.Quantity;
        _productRepository.Update(product);
        await _storage.SaveAsync(cancellationToken);

        return Unit.Value;
    }

    public async ValueTask<Unit> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.Id, cancellationToken);

        if (product == null)
        {
            throw new NotFoundException(ValidationErrors.ProductNotFound);
        }

        _productRepository.Delete(product);
        await _storage.SaveAsync(cancellationToken);

        return Unit.Value;
    }
}
