using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Orders.Domain;
using MediatR;

namespace ECommerce.Orders.Application.Orders.Queries.GetOrderSummaryById;
public record GetOrderSummaryByIdQuery(Guid Id) : IRequest<OrderSummary?>;
