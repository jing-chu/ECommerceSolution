using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Orders.Application.Contracts.Persistence;
using MediatR;

namespace ECommerce.Orders.Application.Products.Commands.UpdateProduct;
public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepo;

    public UpdateProductCommandHandler(IProductRepository repository)
    {
        _productRepo = repository;
    }

    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepo.GetByIdAsync(request.Id, cancellationToken);
        if (product == null)
            throw new KeyNotFoundException("Product not found");

        product.Name = request.Name;
        product.Price = request.Price;
        
        await _productRepo.UpdateAsync(product, cancellationToken);
    }
}
