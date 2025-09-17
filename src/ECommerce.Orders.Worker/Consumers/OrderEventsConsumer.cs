using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ECommerce.Orders.Application.Contracts.Persistence;
using ECommerce.Orders.Application.Orders.Commands.CreateOrder;
using ECommerce.Orders.Contracts.Orders.Events;
using ECommerce.Orders.Domain;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Orders.Worker.Consumers;
public class OrderEventsConsumer : IConsumer<OrderCreatedEvent>, IConsumer<OrderDeletedEvent>, IConsumer<OrderUpdatedEvent>
{
    private readonly IOrderSummaryRepository _orderSummaryRepository;
    private readonly IOrderRepository _orderRepository;
    public OrderEventsConsumer(IOrderSummaryRepository orderSummaryRepository, IOrderRepository orderRepository)
    {
        _orderSummaryRepository = orderSummaryRepository;
        _orderRepository = orderRepository;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        Guid theOrderId = context.Message.OrderId;
        var order = await _orderRepository.GetByIdAsync(theOrderId);
        if (order != null)
        {
            var summary = new OrderSummary
                {
                    Id = order.Id,
                    CustomerId = order.CustomerId,
                    OrderDate = order.OrderDate,
                    TotalPrice = order.TotalPrice,
                    OrderStatus = "Processing",
                    ProductsJson = JsonSerializer.Serialize(order.OrderLines.Select(ol => new
                    {
                        ol.ProductId,
                        ol.Quantity,
                        ol.UnitPrice,
                    }))
                };

            await _orderSummaryRepository.AddAsync(summary);
        }   
    }

    public async Task Consume(ConsumeContext<OrderDeletedEvent> context)
    {
        var orderId = context.Message.OrderId;
        await _orderSummaryRepository.DeleteAsync(orderId);
    }

    public async Task Consume(ConsumeContext<OrderUpdatedEvent> context)
    {
        Guid theOrderId = context.Message.OrderId;
        var updatedOrder = await _orderRepository.GetByIdAsync(theOrderId);
        if (updatedOrder != null )
        {
            var summaryToUpdate = await _orderSummaryRepository.GetByIdAsync(theOrderId);
            if (summaryToUpdate != null)
            {
                summaryToUpdate.CustomerId = updatedOrder.CustomerId;
                summaryToUpdate.OrderDate = updatedOrder.OrderDate;   
                summaryToUpdate.TotalPrice = updatedOrder.TotalPrice;
                summaryToUpdate.OrderStatus = "Updated";
                summaryToUpdate.ProductsJson = JsonSerializer.Serialize(updatedOrder.OrderLines.Select(ol => new
                {
                    ol.ProductId,
                    ol.Quantity,
                    ol.UnitPrice,
                }));
            await _orderSummaryRepository.UpdateAsync(summaryToUpdate);
            }    
        }
    }
}