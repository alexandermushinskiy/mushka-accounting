using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Exhibitions.Converters;
using Mushka.Domain.Extensibility.Repositories;

namespace Mushka.Application.Exhibitions.Queries.Search;

public sealed class SearchExhibitionsQueryHandler : IQueryHandler<SearchExhibitionsQuery, SearchExhibitionsReadModel>
{
    private readonly IExhibitionRepository _exhibitionRepository;
    private readonly ISearchExhibitionsConverter _converter;

    public SearchExhibitionsQueryHandler(IStorage storage, ISearchExhibitionsConverter converter)
    {
        _exhibitionRepository = storage.GetRepository<IExhibitionRepository>();
        _converter = converter;
    }

    public async ValueTask<SearchExhibitionsReadModel> Handle(SearchExhibitionsQuery query, CancellationToken cancellationToken)
    {
        var exhibitions = (await _exhibitionRepository.GetAllAsync(cancellationToken)).ToList();
        return _converter.ToReadModel(exhibitions);
    }
}
