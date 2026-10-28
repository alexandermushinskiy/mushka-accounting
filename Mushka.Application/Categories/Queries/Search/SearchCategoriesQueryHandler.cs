using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Categories.Converters;
using Mushka.Domain.Extensibility.Repositories;

namespace Mushka.Application.Categories.Queries.Search;

public sealed class SearchCategoriesQueryHandler : IQueryHandler<SearchCategoriesQuery, SearchCategoriesReadModel>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISearchCategoriesConverter _converter;

    public SearchCategoriesQueryHandler(IStorage storage, ISearchCategoriesConverter converter)
    {
        _categoryRepository = storage.GetRepository<ICategoryRepository>();
        _converter = converter;
    }

    public async ValueTask<SearchCategoriesReadModel> Handle(SearchCategoriesQuery query, CancellationToken cancellationToken)
    {
        var categories = (await _categoryRepository.GetAllAsync(cancellationToken))
            .OrderByDescending(category => category.Order)
            .ToList();

        return _converter.ToReadModel(categories);
    }
}
