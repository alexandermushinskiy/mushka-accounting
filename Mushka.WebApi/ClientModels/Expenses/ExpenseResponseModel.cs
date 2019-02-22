using System.Collections.Generic;

namespace Mushka.WebApi.ClientModels.Expenses
{
    public class ExpenseResponseModel : ResponseModelBase
    {
        public ExpenseModel Data { get; set; }
    }

    public class ExpensesResponseModel : ResponseModelBase
    {
        public IEnumerable<ExpenseModel> Data { get; set; }
    }
}