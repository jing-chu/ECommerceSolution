using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Orders.Contracts.Orders.Events;
public record OrderCreatedEvent(Guid OrderId);
