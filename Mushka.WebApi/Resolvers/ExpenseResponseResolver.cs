using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Expenses;

namespace Mushka.WebApi.Resolvers
{
    public class ExpenseResponseResolver :
        IValueResolver<OperationResult<Expense>, ExpenseResponseModel, ExpenseModel>,
        IValueResolver<OperationResult<IEnumerable<Expense>>, ExpensesResponseModel, IEnumerable<ExpenseModel>>
    {
        public ExpenseModel Resolve(OperationResult<Expense> source,
            ExpenseResponseModel destination,
            ExpenseModel destMember,
            ResolutionContext context) => source.Data == null ? null : Mapper.Map<Expense, ExpenseModel>(source.Data);

        public IEnumerable<ExpenseModel> Resolve(
            OperationResult<IEnumerable<Expense>> source,
            ExpensesResponseModel destination,
            IEnumerable<ExpenseModel> destMember,
            ResolutionContext context) => source.Data?.Select(Mapper.Map<Expense, ExpenseModel>);
    }
}