using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Orders.Application.Contracts.Persistence;
using ECommerce.Orders.Domain;
using MediatR;

namespace ECommerce.Orders.Application.Orders.Commands.CreateOrder;
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            OrderDate = request.OrderDate,
            TotalPrice = request.TotalPrice,
        };

        order.OrderLines = request.OrderLines.Select(ol => new OrderLine
        {
            Id = Guid.NewGuid(),
            ProductId = ol.ProductId,
            Quantity = ol.Quantity,
            UnitPrice = ol.UnitPrice,
            OrderId = order.Id,
        }).ToList();

        await _orderRepository.AddAsync(order, cancellationToken);
        return order.Id;
    }
}
