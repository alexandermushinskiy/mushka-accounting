using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Domain.Dto;
using Mushka.Domain.Extensibility.Repositories;

namespace Mushka.Application.Analytics.Queries.GetBalance;

public sealed class GetBalanceQueryHandler : IQueryHandler<GetBalanceQuery, GetBalanceReadModel>
{
    private readonly IAnalyticsRepository _analyticsRepository;

    public GetBalanceQueryHandler(IStorage storage)
    {
        _analyticsRepository = storage.GetRepository<IAnalyticsRepository>();
    }

    public async ValueTask<GetBalanceReadModel> Handle(GetBalanceQuery query, CancellationToken cancellationToken)
    {
        var balance = await _analyticsRepository.GetBalance(cancellationToken);

        var readModel = ToReadModel(balance);
        return readModel;
    }

    private static GetBalanceReadModel ToReadModel(Balance balance) =>
        new GetBalanceReadModel
        {
            Data = new BalanceReadModel
            {
                Expense = balance.Expense,
                Profit = balance.Profit
            }
        };
}
