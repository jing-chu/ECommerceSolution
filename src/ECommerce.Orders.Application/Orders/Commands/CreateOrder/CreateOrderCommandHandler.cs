using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ECommerce.Orders.Application.Contracts.Persistence;
using ECommerce.Orders.Domain;
using MediatR;
using MassTransit;
using ECommerce.Orders.Contracts.Orders.Commands;
using ECommerce.Orders.Contracts.Orders.Events;

namespace ECommerce.Orders.Application.Orders.Commands.CreateOrder;
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IPublishEndpoint publishEndpoint)
    {
        _orderRepository = orderRepository;
        _publishEndpoint = publishEndpoint;
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

        //op de schrijf-kant: nadat enige taak uitvoeren en een "event" publiceren
        await _publishEndpoint.Publish(new OrderCreatedEvent(order.Id));

        return order.Id;
    }
}
