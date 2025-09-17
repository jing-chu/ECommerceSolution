using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Orders.Application.Contracts.Persistence;
using ECommerce.Orders.Contracts.Orders.Commands;
using ECommerce.Orders.Contracts.Orders.Events;
using ECommerce.Orders.Domain;
using MassTransit;
using MediatR;

namespace ECommerce.Orders.Application.Orders.Commands.UpdateOrder;
internal class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    public UpdateOrderCommandHandler(IOrderRepository orderRepository, IPublishEndpoint publishEndpoint)
    {
        _orderRepository = orderRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.Id, cancellationToken);

        if (order == null)
            throw new KeyNotFoundException("Order not found");

        order.CustomerId = request.CustomerId;
        order.OrderDate = request.OrderDate;
        order.TotalPrice = request.TotalPrice;

        foreach (var ol in request.OrderLines)
        {
            order.OrderLines.Add(new OrderLine
            {
                ProductId = ol.ProductId,
                Quantity = ol.Quantity,
                UnitPrice = ol.UnitPrice,
                OrderId = order.Id
            });
        }

        await _orderRepository.UpdateAsync(order, cancellationToken);

        //De handler publiceert een OrderDeletedEvent(order.Id) bericht naar RabbitMQ
        await _publishEndpoint.Publish(new OrderUpdatedEvent(order.Id));
    }
}
