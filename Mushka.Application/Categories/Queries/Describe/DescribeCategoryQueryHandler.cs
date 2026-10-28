using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Categories.Converters;
using Mushka.Application.Exceptions;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;

namespace Mushka.Application.Categories.Queries.Describe;

public sealed class DescribeCategoryQueryHandler : IQueryHandler<DescribeCategoryQuery, DescribeCategoryReadModel>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IDescribeCategoryConverter _converter;

    public DescribeCategoryQueryHandler(IStorage storage, IDescribeCategoryConverter converter)
    {
        _categoryRepository = storage.GetRepository<ICategoryRepository>();
        _converter = converter;
    }

    public async ValueTask<DescribeCategoryReadModel> Handle(DescribeCategoryQuery query, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(query.Id, cancellationToken);

        if (category == null)
        {
            throw new NotFoundException(ValidationErrors.CategoryNotFound);
        }

        return _converter.ToReadModel(category);
    }
}
