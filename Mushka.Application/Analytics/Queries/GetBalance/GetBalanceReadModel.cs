namespace Mushka.Application.Analytics.Queries.GetBalance;

public sealed class GetBalanceReadModel
{
    public BalanceReadModel Data { get; set; }
}

public sealed class BalanceReadModel
{
    public int Expense { get; set; }

    public int Profit { get; set; }
}
