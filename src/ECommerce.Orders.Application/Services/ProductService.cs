using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Orders.Application.Contracts.Persistence;
using ECommerce.Orders.Domain;

namespace ECommerce.Orders.Application.Services;
public class ProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product?> GetProductAsync(Guid id)
        => await _productRepository.GetByIdAsync(id);

    public async  Task<IEnumerable<Product>> GetAllProductsAsync()
        => await _productRepository.GetAllAsync();

    public async Task CreateProductAsync(Product product)
        => await _productRepository.AddAsync(product);

    public async Task UpdateProductAsync(Product product)
        => await _productRepository.UpdateAsync(product);

    public async Task DeleteProductAsync(Guid id)
        => await _productRepository.DeleteAsync(id);

}
