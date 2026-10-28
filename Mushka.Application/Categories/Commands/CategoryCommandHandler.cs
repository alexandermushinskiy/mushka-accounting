using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Categories.Converters;
using Mushka.Application.Exceptions;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;

namespace Mushka.Application.Categories.Commands;

public sealed class CategoryCommandHandler :
    ICommandHandler<CreateCategoryCommand>,
    ICommandHandler<UpdateCategoryCommand>,
    ICommandHandler<DeleteCategoryCommand>
{
    private readonly IStorage _storage;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICreateCategoryConverter _createCategoryConverter;
    private readonly IUpdateCategoryConverter _updateCategoryConverter;

    public CategoryCommandHandler(
        IStorage storage,
        ICreateCategoryConverter createCategoryConverter,
        IUpdateCategoryConverter updateCategoryConverter)
    {
        _storage = storage;
        _categoryRepository = storage.GetRepository<ICategoryRepository>();
        _productRepository = storage.GetRepository<IProductRepository>();
        _createCategoryConverter = createCategoryConverter;
        _updateCategoryConverter = updateCategoryConverter;
    }

    public async ValueTask<Unit> Handle(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        if (await _categoryRepository.IsExistAsync(category => category.Name == command.Category.Name, cancellationToken))
        {
            throw new BusinessRuleException(ValidationErrors.CategoryWithNameExist);
        }

        _categoryRepository.Add(_createCategoryConverter.ToCategory(command));
        await _storage.SaveAsync(cancellationToken);

        return Unit.Value;
    }

    public async ValueTask<Unit> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var categoryToUpdate = await _categoryRepository.GetByIdAsync(command.Id, cancellationToken);

        if (categoryToUpdate == null)
        {
            throw new NotFoundException(ValidationErrors.CategoryNotFound);
        }

        if (await _categoryRepository.IsExistAsync(
                category => category.Id != command.Id && category.Name == command.Category.Name,
                cancellationToken))
        {
            throw new BusinessRuleException(ValidationErrors.CategoryWithNameExist);
        }

        var category = _updateCategoryConverter.ToCategory(command);
        category.Order = categoryToUpdate.Order;
        _categoryRepository.Update(category);
        await _storage.SaveAsync(cancellationToken);

        return Unit.Value;
    }

    public async ValueTask<Unit> Handle(DeleteCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(command.Id, cancellationToken);

        if (category == null)
        {
            throw new NotFoundException(ValidationErrors.CategoryNotFound);
        }

        if ((await _productRepository.GetByCategoryId(command.Id, cancellationToken)).Any())
        {
            throw new BusinessRuleException(ValidationErrors.CategoryCannotBeDeletedWithProducts);
        }

        _categoryRepository.Delete(category);
        await _storage.SaveAsync(cancellationToken);

        return Unit.Value;
    }
}
