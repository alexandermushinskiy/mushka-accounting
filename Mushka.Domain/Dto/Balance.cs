namespace Mushka.Domain.Dto
{
    public class Balance
    {
        public decimal Expense { get; }

        public decimal Profit { get; }

        public Balance(decimal expense, decimal profit)
        {
            Expense = expense;
            Profit = profit;
        }
    }
}