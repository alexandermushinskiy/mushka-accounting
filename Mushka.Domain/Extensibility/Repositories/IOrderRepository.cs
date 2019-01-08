﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Domain.Entities;

namespace Mushka.Domain.Extensibility.Repositories
{
    public interface IOrderRepository : IRepositoryBase<Order>
    {
        Task<int> GetSoldCount(Guid productId, CancellationToken cancellationToken = default(CancellationToken));
    }
}