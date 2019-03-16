using System;
using Mushka.Domain.Entities;

namespace Mushka.Domain.Extensibility.Repositories
{
    public interface ICorporateOrderRepository : IRepositoryBase<CorporateOrder>
    {
        void DeleteProducts(Guid corporateOrderId);
    }
}