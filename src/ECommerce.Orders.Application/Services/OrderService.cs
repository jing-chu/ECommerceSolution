using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Orders.Application.Contracts.Persistence;
using ECommerce.Orders.Domain;

namespace ECommerce.Orders.Application.Services;
public class OrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Order?> GetOrderAsync(Guid id)
        => await _orderRepository.GetByIdAsync(id);

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        => await _orderRepository.GetAllAsync();

    public async Task CreateOrderAsync(Order order)
        => await _orderRepository.AddAsync(order);

    public async Task UpdateOrderAsync(Order order)
        => await _orderRepository.UpdateAsync(order);

    public async Task DeleteOrderAsync(Guid id)
        => await _orderRepository.DeleteAsync(id);
}
