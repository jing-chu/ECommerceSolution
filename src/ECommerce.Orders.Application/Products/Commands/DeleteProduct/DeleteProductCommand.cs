using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ECommerce.Orders.Application.Products.Commands.DeleteProduct;
public record DeleteProductCommand(Guid Id) : IRequest;

