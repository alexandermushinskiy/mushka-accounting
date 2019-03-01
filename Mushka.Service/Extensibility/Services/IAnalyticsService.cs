﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;
using Mushka.Domain.Dto;

namespace Mushka.Service.Extensibility.Services
{
    public interface IAnalyticsService
    {
        Task<ValidationResponse<Balance>> GetBalance(CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<IEnumerable<PopularProduct>>> GetPopularProducts(CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<IEnumerable<PopularProduct>>> GetUnpopularProducts(CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<IEnumerable<PopularCity>>> GetPopularCities(CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<IEnumerable<OrdersCount>>> GetOrdersCount(int periodInMonth, CancellationToken cancellationToken = default(CancellationToken));

        Task<ValidationResponse<IEnumerable<SoldProductsCount>>> GetSoldProductsCount(int periodInMonth, CancellationToken cancellationToken = default(CancellationToken));
    }
}