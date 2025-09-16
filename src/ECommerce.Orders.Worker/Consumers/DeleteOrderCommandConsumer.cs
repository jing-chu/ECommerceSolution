using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Orders.Contracts.Orders.Commands;
using MassTransit;
using MediatR;

namespace ECommerce.Orders.Worker.Consumers;
public class DeleteOrderCommandConsumer : IConsumer<DeleteOrderCommand>
{
    private readonly IMediator _mediator;
    public DeleteOrderCommandConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<DeleteOrderCommand> context)
    {
        await _mediator.Send(context.Message);
    }
}


