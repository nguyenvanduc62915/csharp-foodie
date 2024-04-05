using Microsoft.EntityFrameworkCore;

using AppCore.Data;
using AppCore.Models;
using AppCore.Models.ViewModels;
using AppCore.App.Wrapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AppCore.App.Repositories;

public class OrderRepository : IOrderRepository {

    private readonly ApplicationDbContext _dbContext;

    public OrderRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<Order> GetOrderByIdAsync(int orderId) {
        return await _dbContext.Orders.FindAsync(orderId);
    }
    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _dbContext.Orders.ToListAsync();
    }

    public async Task<IEnumerable<OrderRequest>> GetAllOrdersJoinAsync() {
        var result = await (from order in _dbContext.Orders
                        join user in _dbContext.Users on order.UserId equals user.Id
                        join paymentMethod in _dbContext.PaymentMethods on order.PaymentMethodId equals paymentMethod.PaymentMethodId
                        select new OrderRequest {
                            OrderId = order.OrderId,
                            ReceiverName = order.ReceiverName,
                            ReceiverPhoneNumber = order.ReceiverPhoneNumber,
                            ShippingAddress = order.ShippingAddress,
                            UserName = user.UserName,
                            PaymentMethodName = paymentMethod.Name,
                            // Muốn nào tự thêm trường đó vào 
                        }).ToListAsync();
        return result;
    }
    public async Task AddOrderAsync(Order order) {
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateOrderAsync(Order order) {
        _dbContext.Entry(order).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> DeleteOrderAsync(int orderId) {
        var order = await _dbContext.Orders.FindAsync(orderId);
        if (order != null) {
            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }
 
    Task<IEnumerable<Order>> IOrderRepository.GetAllOrdersAsync() {
        throw new NotImplementedException();
    }

    // Lấy sản phẩm ra theo phiên gần nhất 
    public async Task<Order> GetMostRecentOrderAsync(string userId)
    {
        return await _dbContext.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<OrderItemViewModel>> GetOrderItemsByOrderIdAsync(int orderId)
    {
        var orderItems = await (from orderItem in _dbContext.OrderItems
                                join product in _dbContext.Products on orderItem.ProductId equals product.ProductId
                                where orderItem.OrderId == orderId
                                select new OrderItemViewModel
                                {
                                    ProductId = orderItem.ProductId,
                                    ProductName = product.Name,
                                    Image = product.Image,
                                    Price = product.Price,
                                    Quantity = orderItem.Quantity
                                }).ToListAsync();
        return orderItems;
    }
}