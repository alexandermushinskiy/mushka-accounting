using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Exceptions;
using Mushka.Application.Expenses.Converters;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;

namespace Mushka.Application.Expenses.Queries.Describe;

public sealed class DescribeExpenseQueryHandler : IQueryHandler<DescribeExpenseQuery, DescribeExpenseReadModel>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IDescribeExpenseConverter _converter;

    public DescribeExpenseQueryHandler(IStorage storage, IDescribeExpenseConverter converter)
    {
        _expenseRepository = storage.GetRepository<IExpenseRepository>();
        _converter = converter;
    }

    public async ValueTask<DescribeExpenseReadModel> Handle(DescribeExpenseQuery query, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAsync(query.Id, cancellationToken);

        if (expense == null)
        {
            throw new NotFoundException(ValidationErrors.ExpenseNotFound);
        }

        return _converter.ToReadModel(expense);
    }
}
