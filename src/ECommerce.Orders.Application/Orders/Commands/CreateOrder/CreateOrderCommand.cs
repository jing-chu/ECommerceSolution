using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Orders.Domain;
using MediatR;

namespace ECommerce.Orders.Application.Orders.Commands.CreateOrder;
public record CreateOrderCommand(
    string CustomerId,
    DateTime OrderDate,
    decimal TotalPrice,
    List<CreateOrderLineDto> OrderLines
    ) : IRequest<Guid>;

public record CreateOrderLineDto(Guid ProductId, int Quantity, decimal UnitPrice);