using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Orders.Application.Contracts.Persistence;
using ECommerce.Orders.Domain;
using MediatR;

namespace ECommerce.Orders.Application.Orders.Queries.GetAllOrders;
public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<OrderSummary>>
{
    private readonly IOrderSummaryRepository _orderSummaryRepo;
    public GetAllOrdersQueryHandler(IOrderSummaryRepository orderSummaryRepo)
    {
        _orderSummaryRepo = orderSummaryRepo;
    }

    async Task<IEnumerable<OrderSummary>> IRequestHandler<GetAllOrdersQuery, IEnumerable<OrderSummary>>.Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        return await _orderSummaryRepo.GetAllAsync(cancellationToken);
    }
}
