using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Mushka.Core.Extensibility.Providers;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Entities;
using Mushka.Service.Extensibility.Services;
using Mushka.WebApi.ClientModels;
using Mushka.WebApi.ClientModels.Expenses;
using Mushka.WebApi.ClientModels.Expenses.Describe;
using Mushka.WebApi.ClientModels.Expenses.Search;
using Mushka.WebApi.Extensibility.Providers;

namespace Mushka.WebApi.Controllers
{
    [Route("api/v1/[controller]")]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService expenseService;

        public ExpensesController(
            IExpenseService expenseService,
            ICancellationTokenSourceProvider cancellationTokenSourceProvider,
            IActionResultProvider actionResultProvider,
            IMapper mapper)
            : base(cancellationTokenSourceProvider, actionResultProvider, mapper)
        {
            this.expenseService = expenseService;
        }

        [HttpPost("search")]
        public async Task<IActionResult> Search()
        {
            var operationResult = await expenseService.SearchAsync(cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<SearchExpensesResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpGet("{id:guid}/describe")]
        public async Task<IActionResult> Describe(Guid id)
        {
            var operationResult = await expenseService.GetByIdAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<DescribeExpenseResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ExpenseRequestModel expenseRequest)
        {
            var expense = mapper.Map<Expense>(expenseRequest);

            var operationResult = await expenseService.AddAsync(expense, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<EmptyResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] ExpenseRequestModel expenseRequest)
        {
            var expense = mapper.Map<ExpenseRequestModel, Expense>(expenseRequest, opt => opt.Items.Add(nameof(IEntity.Id), id));

            var operationResult = await expenseService.UpdateAsync(expense, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<EmptyResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var operationResult = await expenseService.DeleteAsync(id, cancellationTokenSourceProvider.Get().Token);
            var clientResponse = mapper.Map<EmptyResponseModel>(operationResult);

            return actionResultProvider.GetNew(operationResult, clientResponse);
        }
    }
}