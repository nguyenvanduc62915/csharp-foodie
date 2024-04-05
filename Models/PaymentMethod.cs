using System.ComponentModel.DataAnnotations;

namespace AppCore.Models;

public enum PaymentStatus {
    Pending,
    Shipping,
    Paid,
}
public class PaymentMethod
{
    [Key]
    public int PaymentMethodId { get; set; }
    
    [MaxLength(50)]
    public string Name { get; set; }
    public bool Active { get; set; }
    // Navigation property
    public ICollection<Order> Orders { get; set; }
}