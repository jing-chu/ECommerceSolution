using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Orders.Application.Contracts.Persistence;
using ECommerce.Orders.Domain;
using MediatR;

namespace ECommerce.Orders.Application.Orders.Queries.GetAllOrders;
public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<Order>>
{
    private readonly IOrderRepository _orderRepo;
    public GetAllOrdersQueryHandler(IOrderRepository orderRepo)
    {
        _orderRepo = orderRepo;
    }

    async Task<IEnumerable<Order>> IRequestHandler<GetAllOrdersQuery, IEnumerable<Order>>.Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        return await _orderRepo.GetAllAsync(cancellationToken);
    }
}
