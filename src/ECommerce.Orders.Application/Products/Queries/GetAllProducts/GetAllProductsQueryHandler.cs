using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Orders.Application.Contracts.Persistence;
using ECommerce.Orders.Domain;
using MediatR;

namespace ECommerce.Orders.Application.Products.Queries.GetAllProducts;
public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Product>>
{
    private readonly IProductRepository _productRepo;

    public GetAllProductsQueryHandler(IProductRepository repo)
    {
        _productRepo = repo;
    }

    public async Task<IEnumerable<Product>> Handle (GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        return await _productRepo.GetAllAsync(cancellationToken);
    }
}
