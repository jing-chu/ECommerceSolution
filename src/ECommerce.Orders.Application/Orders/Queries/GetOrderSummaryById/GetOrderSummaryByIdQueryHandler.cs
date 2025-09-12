using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Orders.Application.Contracts.Persistence;
using ECommerce.Orders.Domain;
using MediatR;

namespace ECommerce.Orders.Application.Orders.Queries.GetOrderSummaryById;
public class GetOrderSummaryByIdQueryHandler
    : IRequestHandler<GetOrderSummaryByIdQuery, OrderSummary?>
{
    private readonly IOrderSummaryRepository _orderSummaryRepository;

    public GetOrderSummaryByIdQueryHandler(IOrderSummaryRepository orderSummaryRepository)
    {
        _orderSummaryRepository = orderSummaryRepository;
    }

    public Task<OrderSummary?> Handle(GetOrderSummaryByIdQuery request, CancellationToken cancellationToken)
    {
        return _orderSummaryRepository.GetByIdAsync(request.Id, cancellationToken);
    }
}

