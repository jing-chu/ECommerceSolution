using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ECommerce.Orders.Contracts.Orders.Commands;
public record DeleteOrderCommand(Guid id) : IRequest;
