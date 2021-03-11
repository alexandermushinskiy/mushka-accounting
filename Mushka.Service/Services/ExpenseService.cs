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
using Mushka.Domain.Strings;
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

        public async Task<OperationResult<IEnumerable<Expense>>> SearchAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Expense> expenses = (await expenseRepository.GetAllAsync(cancellationToken))
                .OrderBy(expense => expense.CreatedOn)
                .ToList();

            return OperationResult<IEnumerable<Expense>>.FromResult(expenses);
        }

        public async Task<OperationResult<IEnumerable<Expense>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            IEnumerable<Expense> expenses = (await expenseRepository.GetAllAsync(cancellationToken))
                .OrderBy(expense => expense.CreatedOn)
                .ToList();

            return OperationResult<IEnumerable<Expense>>.FromResult(expenses);
        }

        public async Task<OperationResult<Expense>> GetByIdAsync(Guid expenseId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var expense = await expenseRepository.GetByIdAsync(expenseId, cancellationToken);

            return expense == null
                ? OperationResult<Expense>.FromError(ValidationErrors.ExpenseNotFound, ValidationStatusType.NotFound)
                : OperationResult<Expense>.FromResult(expense);
        }

        public async Task<OperationResult<Expense>> AddAsync(Expense expense, CancellationToken cancellationToken = default(CancellationToken))
        {
            var addedExpense = expenseRepository.Add(expense);
            await storage.SaveAsync(cancellationToken);

            return OperationResult<Expense>.FromResult(addedExpense);
        }

        public async Task<OperationResult<Expense>> UpdateAsync(Expense expense, CancellationToken cancellationToken = default(CancellationToken))
        {
            var expenseToUpdate = await expenseRepository.GetByIdAsync(expense.Id, cancellationToken);

            if (expenseToUpdate == null)
            {
                return OperationResult<Expense>.FromError(ValidationErrors.ExpenseNotFound, ValidationStatusType.NotFound);
            }

            var updatedExpense = expenseRepository.Update(expense);
            await storage.SaveAsync(cancellationToken);

            return OperationResult<Expense>.FromResult(updatedExpense);
        }

        public async Task<OperationResult<Expense>> DeleteAsync(Guid expenseId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var expense = await expenseRepository.GetByIdAsync(expenseId, cancellationToken);

            if (expense == null)
            {
                return OperationResult<Expense>.FromError(ValidationErrors.ExpenseNotFound, ValidationStatusType.NotFound);
            }

            var deletedExpense = expenseRepository.Delete(expense);
            await storage.SaveAsync(cancellationToken);

            return OperationResult<Expense>.FromResult(deletedExpense);
        }
    }
}