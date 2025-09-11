using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Orders.Application.Orders.Commands.CreateOrder;
using MediatR;

namespace ECommerce.Orders.Application.Orders.Commands.DeleteOrder;
public record DeleteOrderCommand(Guid id) : IRequest;
