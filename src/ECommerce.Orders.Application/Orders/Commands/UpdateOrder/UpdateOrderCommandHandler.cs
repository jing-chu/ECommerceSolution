using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Orders.Application.Contracts.Persistence;
using ECommerce.Orders.Domain;
using MediatR;

namespace ECommerce.Orders.Application.Orders.Commands.UpdateOrder;
internal class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    public UpdateOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
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
    }
}
