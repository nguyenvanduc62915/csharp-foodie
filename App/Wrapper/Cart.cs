using AppCore.App.Wrapper;
using AppCore.Extensions;
using AppCore.Models.ViewModels;

namespace AppCore.App.Wrapper;
public class Cart {
    public List<OrderItemViewModel> OrderItems { get; set; }
    public Cart() {
        OrderItems = new List<OrderItemViewModel>();
    }
    public void AddItem(int productId, decimal price, int quantity = 1) {
        // var existingItem = OrderItems.FirstOrDefault(orderItem => orderItem.ProductId == product.ProductId);
        var existingItem = OrderItems.FirstOrDefault(orderItem => orderItem.ProductId == productId);

        if (existingItem != null) {
            existingItem.Quantity += quantity;
            existingItem.Price += price * quantity;
        }
        else {
            OrderItems.Add(new OrderItemViewModel { ProductId = productId, Quantity = 1, Price = price});
        }
    }
    public void RemoveItem(int productId) 
    {
        var itemToRemove = OrderItems.FirstOrDefault(orderItem => orderItem.ProductId == productId);
        if (itemToRemove != null) {
            OrderItems.Remove(itemToRemove);
        }
    }
    public decimal CalculateTotal() {
        return OrderItems.Sum(orderItem => orderItem.Price * orderItem.Quantity);
    }

    public int GetQuantity() {
        return OrderItems.Sum(orderItem => orderItem.Quantity);
    }

    public void Clear() {
        OrderItems.Clear();
    }
    // public List<Product> GetProductsFromOrderItems(List<OrderItem> orderItems)
    // {
    //     return products;
    // }
    
    // private void SaveCart() {
    //     // var session = HttpContext.Session;
    //     // HttpContext.Session.Set<Cart>("Cart", this);
    // }
}

