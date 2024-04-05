namespace AppCore.Models.ViewModels;

public class OrderRequest {
    public int OrderId { get; set; }
    
    public string UserId { get; set; }

    public string UserName { get; set; }
    public string ReceiverName { get; set; }

    public string ReceiverPhoneNumber { get; set; }

    public int PaymentMethodId { get; set; }

    public string PaymentMethodName {get; set;}
    public decimal TotalAmount { get; set; }

    public string Status { get; set; }

    public string ShippingAddress { get; set; }

    public List<OrderItem> OrderItems { get; set; }
    public PaymentStatus PaymentStatus { get; set; }


}