﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;
using Mushka.Service.Extensibility.Dto;

namespace Mushka.Service.Extensibility.Services
{
    public interface IOrderService : IServiceBase<Order>
    {
        Task<OperationResult<IEnumerable<Order>>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<OperationResult<bool>> IsNumberExistAsync(string orderNumber, CancellationToken cancellationToken = default(CancellationToken));

        Task<OperationResult<ExportedFile>> ExportAsync(string title, IEnumerable<Guid> orderIds, CancellationToken cancellationToken = default(CancellationToken));
    }
}