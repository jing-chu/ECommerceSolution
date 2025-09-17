using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Orders.Domain;

namespace ECommerce.Orders.Application.Contracts.Persistence;
public interface IOrderSummaryRepository
{
    Task AddAsync(OrderSummary orderSummary, CancellationToken cancellationToken = default);
    Task<IEnumerable<OrderSummary>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<OrderSummary?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(OrderSummary orderSummary, CancellationToken cancellationToken = default);

}
