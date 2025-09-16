using ECommerce.Orders.Contracts.Orders.Commands;
using ECommerce.Orders.Application.Orders.Queries.GetAllOrders;
using ECommerce.Orders.Application.Orders.Queries.GetOrderById;
using ECommerce.Orders.Application.Orders.Queries.GetOrderSummaryById;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Orders.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{

    private readonly IMediator _mediator;
    private readonly IPublishEndpoint _publishEndpoint;

    public OrdersController(IMediator mediator, IPublishEndpoint publishEndpoint)
    {
        _mediator = mediator;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _mediator.Send(new GetAllOrdersQuery());
        return orders is null ? NotFound() : Ok(orders);
    }

    [HttpGet("{id}/summary")]
    public async Task<IActionResult> GetSummary(Guid id)
    {
        var order = await _mediator.Send(new GetOrderSummaryByIdQuery(id));
        return order is null ? NotFound() : Ok(order);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var order = await _mediator.Send(new GetOrderByIdQuery(id));
        return order is null ? NotFound() : Ok(order);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand command)
    {
        await _publishEndpoint.Publish(command);
        return Accepted();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOrderCommand command)
    {
        if (id != command.Id)
            return BadRequest("Id mismatch");

        try
        {
            await _mediator.Send(command);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _publishEndpoint.Publish(new DeleteOrderCommand(id));
            return Accepted();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
