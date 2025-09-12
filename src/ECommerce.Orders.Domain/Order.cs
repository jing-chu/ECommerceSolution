using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Orders.Domain;
public class Order
{
    public Guid Id { get; set; }
    public string CustomerId { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();

    // Eigenschappen voor Soft Delete
    public bool IsDeleted { get; private set; } = false;
    public DateTime? DeletedAtUtc { get; private set; }
    public void SoftDelete()
    {
        IsDeleted = true;
        DeletedAtUtc = DateTime.UtcNow;
    }
}