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
    public async Task AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }
       

    public async Task DeleteAsync(Guid id)
    {
        var order = await _context.Orders.Include(o => o.OrderLines).FirstOrDefaultAsync(o => o.Id == id);
        if (order == null)
            throw new KeyNotFoundException("Order not found");
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
        => await _context.Orders
        .Include(o => o.OrderLines).ThenInclude(ol => ol.Product)
        .ToListAsync();

    public async Task<Order?> GetByIdAsync(Guid id)
        => await _context.Orders.FindAsync(id);      

    public async Task UpdateAsync(Order order)
    {
        var existing = await _context.Orders.FindAsync(order.Id);
        if (existing == null)
            throw new KeyNotFoundException("Product not found");

        existing.CustomerId = order.CustomerId;
        existing.OrderDate = order.OrderDate;
        existing.TotalPrice = order.TotalPrice;
        existing.OrderLines = order.OrderLines;

        await _context.SaveChangesAsync();
    }
}
