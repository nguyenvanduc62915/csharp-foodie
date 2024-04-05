using AppCore.Models;
using AppCore.Models.ViewModels;
namespace AppCore.App.Repositories;
public interface IOrderRepository {
    Task<Order> GetOrderByIdAsync(int orderId);
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task AddOrderAsync(Order order);
    Task UpdateOrderAsync(Order order);
    Task<bool> DeleteOrderAsync(int orderId);
    // Lất sản phẩm đặt theo phiên gần nhất
    Task<Order> GetMostRecentOrderAsync(string userId);
}