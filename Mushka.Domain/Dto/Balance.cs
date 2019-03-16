namespace Mushka.Domain.Dto
{
    public class Balance
    {
        public int Expense { get; }

        public int Profit { get; }

        public Balance(int expense, int profit)
        {
            Expense = expense;
            Profit = profit;
        }
    }
}