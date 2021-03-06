﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mushka.Core.Validation;
using Mushka.Domain.Entities;

namespace Mushka.Service.Extensibility.Services
{
    public interface IExhibitionService : IServiceBase<Exhibition>
    {
        Task<OperationResult<IEnumerable<Exhibition>>> SearchAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}