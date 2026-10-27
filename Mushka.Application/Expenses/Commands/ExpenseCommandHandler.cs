using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Mushka.Application.Exceptions;
using Mushka.Application.Expenses.Converters;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Domain.Strings;

namespace Mushka.Application.Expenses.Commands;

public sealed class ExpenseCommandHandler :
    ICommandHandler<CreateExpenseCommand>,
    ICommandHandler<UpdateExpenseCommand>,
    ICommandHandler<DeleteExpenseCommand>
{
    private readonly IStorage _storage;
    private readonly IExpenseRepository _expenseRepository;
    private readonly ICreateExpenseConverter _createExpenseConverter;
    private readonly IUpdateExpenseConverter _updateExpenseConverter;

    public ExpenseCommandHandler(
        IStorage storage,
        ICreateExpenseConverter createExpenseConverter,
        IUpdateExpenseConverter updateExpenseConverter)
    {
        _storage = storage;
        _expenseRepository = storage.GetRepository<IExpenseRepository>();
        _createExpenseConverter = createExpenseConverter;
        _updateExpenseConverter = updateExpenseConverter;
    }

    public async ValueTask<Unit> Handle(CreateExpenseCommand command, CancellationToken cancellationToken)
    {
        _expenseRepository.Add(_createExpenseConverter.ToExpense(command));
        await _storage.SaveAsync(cancellationToken);

        return Unit.Value;
    }

    public async ValueTask<Unit> Handle(UpdateExpenseCommand command, CancellationToken cancellationToken)
    {
        var expenseToUpdate = await _expenseRepository.GetByIdAsync(command.Id, cancellationToken);

        if (expenseToUpdate == null)
        {
            throw new NotFoundException(ValidationErrors.ExpenseNotFound);
        }

        _expenseRepository.Update(_updateExpenseConverter.ToExpense(command));
        await _storage.SaveAsync(cancellationToken);

        return Unit.Value;
    }

    public async ValueTask<Unit> Handle(DeleteExpenseCommand command, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAsync(command.Id, cancellationToken);

        if (expense == null)
        {
            throw new NotFoundException(ValidationErrors.ExpenseNotFound);
        }

        _expenseRepository.Delete(expense);
        await _storage.SaveAsync(cancellationToken);

        return Unit.Value;
    }
}
