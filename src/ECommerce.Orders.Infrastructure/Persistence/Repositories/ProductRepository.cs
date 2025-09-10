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

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(o => o.Id == id);
        if (product == null)
            throw new KeyNotFoundException("Product not found");
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

    }

    public async Task<IEnumerable<Product>> GetAllAsync()
        => await _context.Products.AsNoTracking().ToListAsync();

    public async Task<Product?> GetByIdAsync(Guid id)
        => await _context.Products.FindAsync(id);

    public async Task UpdateAsync(Product product)
    {
        var existing = await _context.Products.FindAsync(product.Id);
        if (existing == null)
            throw new KeyNotFoundException("Product not found");

        existing.Name = product.Name;
        existing.Price = product.Price;

        await _context.SaveChangesAsync();
    }
}
