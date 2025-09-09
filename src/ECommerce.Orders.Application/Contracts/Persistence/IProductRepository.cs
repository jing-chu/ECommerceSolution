using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Orders.Domain;

namespace ECommerce.Orders.Application.Contracts.Persistence;
public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(Guid id);

    Task AddAsync(Product product);
    Task DeleteAsync(Guid Id);
    Task UpdateAsync(Product product);
}

