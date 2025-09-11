using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Orders.Application.Contracts.Persistence;
using MediatR;

namespace ECommerce.Orders.Application.Products.Commands.DeleteProduct;
public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _productRepository;
    public DeleteProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var existing = await _productRepository.GetByIdAsync(request.Id, cancellationToken);
        if (existing == null)
            throw new KeyNotFoundException("Product not found");

        existing.SoftDelete();
        await _productRepository.UpdateAsync(existing, cancellationToken);
    }
}
