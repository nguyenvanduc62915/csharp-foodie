using System.ComponentModel.DataAnnotations;

namespace AppCore.Models;

public class OrderItem {
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public DateTime CreatedAt{ get; set; }
    public DateTime UpdatedAt{ get; set; }

    // Navigation properties
    public Order Order { get; set; }
    public Product Product { get; set; }
}