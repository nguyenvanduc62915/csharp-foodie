using System.ComponentModel.DataAnnotations;

namespace AppCore.Models;
public class Order {
    [Key]
    public int OrderId { get; set; }

    public string UserId { get; set; }
    [MaxLength(200)]
    public string ReceiverName { get; set; }
    [MaxLength(50)]
    public string ReceiverPhoneNumber { get; set; }

    public int PaymentMethodId { get; set; }

    public decimal TotalAmount { get; set; }

    [MaxLength(50)]
    public string Status { get; set; }

    [MaxLength(500)]
    public string ShippingAddress { get; set; }

    public PaymentStatus PaymentStatus { get; set; }

    public DateTime CreatedAt{ get; set; }
    public DateTime UpdatedAt{ get; set; }

    public User User { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public List<OrderItem> OrderItems { get; set; }
}