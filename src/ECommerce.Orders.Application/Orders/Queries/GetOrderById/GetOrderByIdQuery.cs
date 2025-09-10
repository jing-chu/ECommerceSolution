using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Orders.Domain;
using MediatR;

namespace ECommerce.Orders.Application.Orders.Queries.GetOrderById;
public record GetOrderByIdQuery(Guid Id) : IRequest<Order?>;

