
namespace AppCore.Models;

public class PaymentMethodRequest {
    public int PaymentMethodId { get; set; }
    public string Name { get; set; }
    public bool Active { get; set; }
    // Navigation property
}