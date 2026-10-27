using System.Collections.Generic;
using Mushka.Application.Exhibitions.Queries.Search;
using Mushka.Domain.Entities;

namespace Mushka.Application.Exhibitions.Converters;

public interface ISearchExhibitionsConverter
{
    SearchExhibitionsReadModel ToReadModel(IEnumerable<Exhibition> exhibitions);
}
