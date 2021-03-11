using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Expenses.Search
{
    public class SearchExpensesResponseModel
    {
        public int Total { get; set; }
        public IEnumerable<SearchExpenseModel> Items { get; set; }
    }
}