using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

using AppCore.Filters;
using AppCore.Models;
using AppCore.Models.ViewModels;
using AppCore.App.Repositories;
using AppCore.Data;
using Microsoft.EntityFrameworkCore;
using static NuGet.Packaging.PackagingConstants;
namespace AppCore.Controllers;


[AuthorizeAdmin]
public class OrderController : Controller {
    private readonly ILogger<OrderController> _logger;
    private OrderRepository _orderRepository;

     private readonly ApplicationDbContext _dbContext;
    public OrderController(ILogger<OrderController> logger, OrderRepository orderRepository, ApplicationDbContext dbContext) {
        _logger = logger;
        _orderRepository = orderRepository;
        _dbContext = dbContext;
    }

    [Route("Admin/Order")]
    public async Task<IActionResult> Index()
    {
        var orders = await _orderRepository.GetAllOrdersAsync();

        var ordersTodayCount = GetOrdersCountForDate(DateTime.Today);   //tạo biến
        var totalOrdersAmount = await GetTotalOrdersAmount();           //tạo biến
        ViewBag.TotalOrdersAmount = totalOrdersAmount;              //Tổng tiền
        ViewBag.TotalOrdersToday = ordersTodayCount;                //Số đơn hàng đã đặt hôm nay
        ViewBag.TotalOrders = orders.Count();                       //Tổng đơn hàng

        return View("~/Views/Admin/Order/Index.cshtml", orders);
    }

    [Route("Admin/Order/Detail/{orderId}")]
    public async Task<IActionResult> Detail(int orderId) {
        var orderItems = from orderItem in _dbContext.OrderItems
                        join product in _dbContext.Products on orderItem.ProductId equals product.ProductId
                        // join order in _dbContext.Orders on orderItem.OrderId equals order.OrderId
                        where orderItem.OrderId == orderId 
                        select new OrderItemViewModel {
                            // OrderId = order.OrderId,
                            ProductId = orderItem.ProductId,
                            ProductName = product.Name,
                            Image = product.Image,
                            Price = product.Price,
                            Quantity = orderItem.Quantity
                        };
        var order = await _orderRepository.GetOrderByIdAsync(orderId);

        ViewData["OrderItems"] = await orderItems.ToListAsync();
        ViewData["Amount"] = order.TotalAmount;
        ViewData["orderId"] = orderId;
        return View("~/Views/Admin/Order/Detail.cshtml");
    }
    // 
    [Route("Admin/Order/Edit/{orderId}")]
    public async Task<IActionResult> Edit(int orderId)
    {
        var order = await _orderRepository.GetOrderByIdAsync(orderId);
        ViewData["Order"] = order;
        if (order == null)
        {
            return NotFound();
        }

        return View("~/Views/Admin/Order/Edit.cshtml", order);
    }
    // Chỉ cập nhật trạng thái status 
    [HttpPost]
    [Route("Admin/Order/UpdateStatus/{orderId}")]
    public async Task<IActionResult> UpdateStatus(int orderId, Order updatedOrder)
    {
        var existingOrder = await _orderRepository.GetOrderByIdAsync(orderId);

        if (existingOrder == null)
        {
            return NotFound();
        }

        existingOrder.Status = updatedOrder.Status;

        await _orderRepository.UpdateOrderAsync(existingOrder);

        TempData["Message"] = "Sửa thành công";
        return RedirectToAction("Index");
    }
    //Hàm đếm số lượng order theo ngày
    public int GetOrdersCountForDate(DateTime date)
    {
        var orders = _orderRepository.GetAllOrdersAsync().Result; 
        var ordersCount = orders.Count(order => order.CreatedAt.Date == date.Date); 
        return ordersCount;
    }
    //Hàm tính tổng tiền trong Order
    public async Task<decimal> GetTotalOrdersAmount()
    {

        var paidOrders = await _orderRepository.GetAllOrdersAsync();
        // Lọc chỉ những đơn hàng có trạng thái là "Paid"
        paidOrders = paidOrders.Where(order => order.Status == "Pending").ToList();
        decimal totalAmount = 0;
        foreach (var order in paidOrders)
        {
            totalAmount += order.TotalAmount;
        }

        return totalAmount;
    }
}
