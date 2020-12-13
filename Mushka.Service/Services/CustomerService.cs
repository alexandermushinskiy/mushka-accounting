using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Extensibility.Logging;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.Domain.Extensibility.Repositories;
using Mushka.Service.Extensibility.Services;

namespace Mushka.Service.Services
{
    internal class CustomerService : ServiceBase, ICustomerService
    {
        private readonly ICustomerRepository customerRepository;

        public CustomerService(
            IStorage storage,
            ILoggerFactory loggerFactory) : base(loggerFactory)
        {
            customerRepository = storage.GetRepository<ICustomerRepository>();
        }

        public async Task<OperationResult<IEnumerable<Customer>>> GetByNameAsync(string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            var customers = (await customerRepository.GetByNameAsync(name, cancellationToken)).ToList();
            
            return OperationResult<IEnumerable<Customer>>.FromResult(customers);
        }
    }
}