using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Orders.Application.Contracts.Persistence;
using ECommerce.Orders.Domain;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Orders.Infrastructure.Persistence.Repositories;
public class OrderSummaryRepository : IOrderSummaryRepository
{
    private readonly AppDbContext _context;
    public OrderSummaryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(OrderSummary orderSummary, CancellationToken cancellationToken = default)
    {
        await _context.OrderSummaries.AddAsync(orderSummary, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var summary = await _context.OrderSummaries.FindAsync(id);
        if(summary != null)
        {
            _context.OrderSummaries.Remove(summary);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<OrderSummary>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.OrderSummaries
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<OrderSummary?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.OrderSummaries.FindAsync(id, cancellationToken);
    }

    public async Task UpdateAsync(OrderSummary orderSummary, CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
