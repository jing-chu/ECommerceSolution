using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Orders.Domain;
using MediatR;

namespace ECommerce.Orders.Application.Orders.Queries.GetAllOrders;
public record GetAllOrdersQuery() : IRequest<IEnumerable<Order>>;
