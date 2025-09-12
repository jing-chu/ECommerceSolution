using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Orders.Domain;
public class OrderSummary
{
    public Guid Id { get; set; } // Heeft dezelfde Id als de Order
    public string CustomerId { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public string OrderStatus { get; set; } = string.Empty; // bv. "Processing", "Completed"
    public string ProductsJson { get; set; } = string.Empty;
}
