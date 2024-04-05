using AppCore.Models;
namespace AppCore.App.Repositories;
public interface IOrderItemRepository {
    Task<OrderItem> GetOrderItemByIdAsync(int orderItemId);
    Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync();
    Task AddOrderItemAsync(OrderItem orderItem);
    Task UpdateOrderItemAsync(OrderItem orderItem);
    Task<bool> DeleteOrderItemAsync(int orderItemId);
}