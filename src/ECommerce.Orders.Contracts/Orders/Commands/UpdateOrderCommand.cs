using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ECommerce.Orders.Contracts.Orders.Commands;
public record UpdateOrderCommand(
    Guid Id,
    string CustomerId,
    DateTime OrderDate,
    decimal TotalPrice,
    List<UpdateOrderLineDto> OrderLines
    ) : IRequest;

public record UpdateOrderLineDto(Guid ProductId, int Quantity, decimal UnitPrice);
