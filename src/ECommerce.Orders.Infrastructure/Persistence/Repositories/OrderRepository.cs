using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Orders.Application.Contracts.Persistence;
using ECommerce.Orders.Domain;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Orders.Infrastructure.Persistence.Repositories;
public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders.Include(o => o.OrderLines).FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        if (order == null)
            throw new KeyNotFoundException("Order not found");
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Orders
        .Where(o => !o.IsDeleted)
        .Include(o => o.OrderLines).ThenInclude(ol => ol.Product)
        .ToListAsync(cancellationToken);

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Orders
        .Where(o => !o.IsDeleted)
        .Include(o => o.OrderLines)
        .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);      

    public async Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        // Omdat de 'order' al wordt gevolgd door de DbContext (opgehaald via GetByIdAsync)
        // EF Core is slim genoeg om te zien welke properties zijn veranderd en genereert
        // een efficiënt SQL statement (bv. UPDATE Orders SET IsDeleted = 1 WHERE ...).
        await _context.SaveChangesAsync(cancellationToken);
    }
}
