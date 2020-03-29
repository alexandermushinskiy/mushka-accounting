using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Extensibility.Logging;
using Mushka.Core.Validation;
using Mushka.Core.Validation.Enums;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Service.Extensibility.Services;

namespace Mushka.Service.Services
{
    internal class ExpenseService : ServiceBase<Expense>, IExpenseService
    {
        private readonly IStorage storage;
        private readonly IExpenseRepository expenseRepository;

        public ExpenseService(
            IStorage storage,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this.storage = storage;

            expenseRepository = storage.GetRepository<IExpenseRepository>();
        }

        public async Task<ValidationResponse<IEnumerable<Expense>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Expense> expenses = (await expenseRepository.GetAllAsync(cancellationToken))
                .OrderBy(expense => expense.CreatedOn)
                .ToList();

            var message = expenses.Any()
                ? "Expenses were successfully retrieved."
                : "No expenses found.";

            return CreateInfoValidationResponse(expenses, message);
        }

        public async Task<ValidationResponse<Expense>> GetByIdAsync(Guid expenseId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var expense = await expenseRepository.GetByIdAsync(expenseId, cancellationToken);

            return expense == null
                ? CreateErrorValidationResponse($"Expense with id {expenseId} is not found.", ValidationStatusType.NotFound)
                : CreateInfoValidationResponse(expense, $"Expense with id {expenseId} was successfully retrieved.");
        }

        public async Task<ValidationResponse<Expense>> AddAsync(Expense expense, CancellationToken cancellationToken = default(CancellationToken))
        {
            var addedExpense = expenseRepository.Add(expense);
            await storage.SaveAsync(cancellationToken);

            return CreateInfoValidationResponse(addedExpense, $"Expense with id {expense.Id} was successfully added.");
        }

        public async Task<ValidationResponse<Expense>> UpdateAsync(Expense expense, CancellationToken cancellationToken = default(CancellationToken))
        {
            var expenseToUpdate = await expenseRepository.GetByIdAsync(expense.Id, cancellationToken);

            if (expenseToUpdate == null)
            {
                return CreateErrorValidationResponse($"Expense with id {expense.Id} is not found.", ValidationStatusType.NotFound);
            }
            
            var updatedExpense = expenseRepository.Update(expense);
            await storage.SaveAsync(cancellationToken);

            return CreateInfoValidationResponse(updatedExpense, $"Expense with id {expense.Id} was successfully updated.");
        }

        public async Task<ValidationResponse<Expense>> DeleteAsync(Guid expenseId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var expense = await expenseRepository.GetByIdAsync(expenseId, cancellationToken);

            if (expense == null)
            {
                return CreateErrorValidationResponse($"Expense with id {expenseId} is not found.", ValidationStatusType.NotFound);
            }

            expenseRepository.Delete(expense);
            await storage.SaveAsync(cancellationToken);

            return CreateInfoValidationResponse(expense, $"Expense with id {expense.Id} was successfully deleted.");
        }
    }
}