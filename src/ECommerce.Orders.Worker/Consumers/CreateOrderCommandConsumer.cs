using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Orders.Application.Orders.Commands.CreateOrder;
using MassTransit;
using MediatR;

namespace ECommerce.Orders.Worker.Consumers;
public class CreateOrderCommandConsumer : IConsumer<CreateOrderCommand>
{
    private readonly IMediator _mediator;
    public CreateOrderCommandConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<CreateOrderCommand> context)
    {
        // De consumer ontvangt het bericht van RabbitMQ en stuurt het
        // naar de MediatR handler die al het werk doet.
        await _mediator.Send(context.Message);
    }
}
