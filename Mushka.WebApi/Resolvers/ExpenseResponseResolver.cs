using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.WebApi.ClientModels.Expenses;

namespace Mushka.WebApi.Resolvers
{
    public class ExpenseResponseResolver :
        IValueResolver<ValidationResponse<Expense>, ExpenseResponseModel, ExpenseModel>,
        IValueResolver<ValidationResponse<IEnumerable<Expense>>, ExpensesResponseModel, IEnumerable<ExpenseModel>>
    {
        public ExpenseModel Resolve(
            ValidationResponse<Expense> source,
            ExpenseResponseModel destination,
            ExpenseModel destMember,
            ResolutionContext context) => source.Result == null ? null : Mapper.Map<Expense, ExpenseModel>(source.Result);

        public IEnumerable<ExpenseModel> Resolve(
            ValidationResponse<IEnumerable<Expense>> source,
            ExpensesResponseModel destination,
            IEnumerable<ExpenseModel> destMember,
            ResolutionContext context) => source.Result?.Select(Mapper.Map<Expense, ExpenseModel>);
    }
}