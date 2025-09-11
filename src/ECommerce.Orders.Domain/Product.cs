using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Orders.Domain;
public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; } = true; 

    public void SoftDelete()
    {
        IsAvailable = false;
    }
}
