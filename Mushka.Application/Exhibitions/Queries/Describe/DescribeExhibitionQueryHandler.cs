using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Exceptions;
using Mushka.Application.Exhibitions.Converters;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;

namespace Mushka.Application.Exhibitions.Queries.Describe;

public sealed class DescribeExhibitionQueryHandler : IQueryHandler<DescribeExhibitionQuery, DescribeExhibitionReadModel>
{
    private readonly IExhibitionRepository _exhibitionRepository;
    private readonly IDescribeExhibitionConverter _converter;

    public DescribeExhibitionQueryHandler(IStorage storage, IDescribeExhibitionConverter converter)
    {
        _exhibitionRepository = storage.GetRepository<IExhibitionRepository>();
        _converter = converter;
    }

    public async ValueTask<DescribeExhibitionReadModel> Handle(DescribeExhibitionQuery query, CancellationToken cancellationToken)
    {
        var exhibition = await _exhibitionRepository.GetByIdAsync(query.Id, cancellationToken);

        if (exhibition == null)
        {
            throw new NotFoundException(ValidationErrors.ExhibitionNotFound);
        }

        return _converter.ToReadModel(exhibition);
    }
}
