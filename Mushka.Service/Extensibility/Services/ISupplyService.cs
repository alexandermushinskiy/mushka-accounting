﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.Domain.Models;
using Mushka.Service.Extensibility.Dto;

namespace Mushka.Service.Extensibility.Services
{
    public interface ISupplyService : IServiceBase<Supply>
    {
        Task<OperationResult<ItemsWithTotalCount<Supply>>> SearchAsync(SearchSuppliesFilter searchSuppliesFilter, CancellationToken cancellationToken = default(CancellationToken));

        Task<OperationResult<ExportedFile>> ExportAsync(IEnumerable<Guid> supplyIds, IEnumerable<Guid> productIds, CancellationToken cancellationToken = default(CancellationToken));
    }
}