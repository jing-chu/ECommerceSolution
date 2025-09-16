using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Orders.Application.Contracts.Persistence;
using ECommerce.Orders.Contracts.Orders.Commands;
using MediatR;

namespace ECommerce.Orders.Application.Orders.Commands.DeleteOrder;
internal class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    public DeleteOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var existing = await _orderRepository.GetByIdAsync(request.id, cancellationToken);
        if (existing == null)
            throw new KeyNotFoundException("Order not found");
        existing.SoftDelete();
        await _orderRepository.UpdateAsync(existing, cancellationToken);

        //Todo: De handler publiceert een OrderDeletedEvent(order.Id) bericht naar RabbitMQ
    }
}
