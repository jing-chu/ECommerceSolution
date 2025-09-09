using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ECommerce.Orders.Domain;
public class OrderLine
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    [JsonIgnore]            // voorkomt cycle
    public Order? Order { get; set; } // Navigation property
    public Guid ProductId { get; set; }
    [JsonIgnore]
    public Product? Product { get; set; } // Navigation property
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; } 
}