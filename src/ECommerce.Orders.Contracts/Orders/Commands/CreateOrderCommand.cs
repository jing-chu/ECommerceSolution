using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ECommerce.Orders.Contracts.Orders.Commands;
public record CreateOrderCommand(
    string CustomerId,
    DateTime OrderDate,
    decimal TotalPrice,
    List<CreateOrderLineDto> OrderLines
    ) : IRequest<Guid>;

public record CreateOrderLineDto(Guid ProductId, int Quantity, decimal UnitPrice);