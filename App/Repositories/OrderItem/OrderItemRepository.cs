using Microsoft.EntityFrameworkCore;

using AppCore.Data;
using AppCore.Models;

namespace AppCore.App.Repositories;

public class OrderItemRepository : IOrderItemRepository {

    private readonly ApplicationDbContext _dbContext;
    public OrderItemRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<OrderItem> GetOrderItemByIdAsync(int orderItemId) {
        return await _dbContext.OrderItems.FindAsync(orderItemId);
    }

    public async Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync() {
        return await _dbContext.OrderItems.ToListAsync();
    }
    
    public async Task AddOrderItemAsync(OrderItem orderItem) {
        _dbContext.OrderItems.Add(orderItem);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateOrderItemAsync(OrderItem orderItem) {
        _dbContext.Entry(orderItem).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteOrderItemAsync(int orderItemId) {
        var orderItem = await _dbContext.OrderItems.FindAsync(orderItemId);
        if (orderItem != null) {
            _dbContext.OrderItems.Remove(orderItem);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }
 
    Task<IEnumerable<OrderItem>> IOrderItemRepository.GetAllOrderItemsAsync() {
        throw new NotImplementedException();
    }

}