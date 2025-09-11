using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Orders.Application.Contracts.Persistence;
using ECommerce.Orders.Domain;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Orders.Infrastructure.Persistence.Repositories;
public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;
    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _context.Products.FirstOrDefaultAsync(o => o.Id == id);
        if (product == null)
            throw new KeyNotFoundException("Product not found");
        _context.Products.Remove(product);
        await _context.SaveChangesAsync(cancellationToken);

    }

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Products
        .Where(p => p.IsAvailable)
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Products
        .Where(p => p.IsAvailable)
        .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
