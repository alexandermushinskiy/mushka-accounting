using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Expenses.Converters;
using Mushka.Domain.Extensibility.Repositories;

namespace Mushka.Application.Expenses.Queries.Search;

public sealed class SearchExpensesQueryHandler : IQueryHandler<SearchExpensesQuery, SearchExpensesReadModel>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly ISearchExpensesConverter _converter;

    public SearchExpensesQueryHandler(IStorage storage, ISearchExpensesConverter converter)
    {
        _expenseRepository = storage.GetRepository<IExpenseRepository>();
        _converter = converter;
    }

    public async ValueTask<SearchExpensesReadModel> Handle(SearchExpensesQuery query, CancellationToken cancellationToken)
    {
        var expenses = (await _expenseRepository.GetAllAsync(cancellationToken))
            .OrderBy(expense => expense.CreatedOn)
            .ToList();

        return _converter.ToReadModel(expenses);
    }
}
