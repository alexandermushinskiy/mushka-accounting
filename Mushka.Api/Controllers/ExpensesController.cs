using System;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Mushka.Api.ClientModels.Expense;
using Mushka.Api.Converters.Expenses;
using Mushka.Application.Expenses.Commands;
using Mushka.Application.Expenses.Queries.Describe;
using Mushka.Application.Expenses.Queries.Search;

namespace Mushka.Api.Controllers;

[Route("api/v1/expenses")]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseRequestConverter _expenseRequestConverter;

    public ExpensesController(ISender sender, IExpenseRequestConverter expenseRequestConverter)
        : base(sender)
    {
        _expenseRequestConverter = expenseRequestConverter;
    }

    [HttpPost("search")]
    public async Task<IActionResult> Search(CancellationToken cancellationToken)
    {
        var expenses = await sender.Send(new SearchExpensesQuery(), cancellationToken);
        return Ok(expenses);
    }

    [HttpGet("{id:guid}/describe")]
    public async Task<IActionResult> Describe(Guid id, CancellationToken cancellationToken)
    {
        var expense = await sender.Send(new DescribeExpenseQuery(id), cancellationToken);
        return Ok(expense);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateExpenseRequestModel request, CancellationToken cancellationToken)
    {
        await sender.Send(_expenseRequestConverter.ToCreateCommand(request), cancellationToken);
        return Ok();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateExpenseRequestModel request, CancellationToken cancellationToken)
    {
        await sender.Send(_expenseRequestConverter.ToUpdateCommand(id, request), cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await sender.Send(new DeleteExpenseCommand(id), cancellationToken);
        return Ok();
    }
}
