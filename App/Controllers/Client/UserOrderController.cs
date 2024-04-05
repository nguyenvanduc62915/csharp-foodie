using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AppCore.Filters;
using AppCore.Models;
using AppCore.Models.ViewModels;
using AppCore.App.Repositories;
using AppCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace AppCore.Controllers;


public class UserOrderController : Controller
{
    private readonly ILogger<UserOrderController> _logger;
    private OrderRepository _orderRepository;
    private readonly UserManager<Account> _userManager;
    private CategoryRepository _categoryRepository;
    private readonly ApplicationDbContext _dbContext;
    public UserOrderController(ILogger<UserOrderController> logger, OrderRepository orderRepository, UserManager<Account> userManager, ApplicationDbContext dbContext, CategoryRepository categoryRepository)
    {
        _userManager = userManager;
        _logger = logger;
        _orderRepository = orderRepository;
        _dbContext = dbContext;
        _categoryRepository = categoryRepository;
    }
	[Route("Admin/UserOrder")]
	public async Task<IActionResult> Index()
	{
		var orders = await _orderRepository.GetAllOrdersAsync();
		return View("~/Views/UserOrder/Index.cshtml", orders);
	}
	[Route("UserOrder/Detail")]
    public async Task<IActionResult> Detail()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        // Lấy đơn hàng mới nhất của người dùng trong phiên làm việc hiện tại
        var mostRecentOrder = await _orderRepository.GetMostRecentOrderAsync(user.Id);
        // Kiểm tra xem có đơn hàng hay không
        if (mostRecentOrder == null)
        {
            // Không có đơn hàng nào, có thể chuyển hướng hoặc hiển thị thông báo tùy ý
            return NotFound();
        }
        // Lấy thông tin chi tiết đơn hàng và hiển thị
        var orderId = mostRecentOrder.OrderId;
        var orderItems = await _orderRepository.GetOrderItemsByOrderIdAsync(orderId);
        ViewData["OrderItems"] = orderItems;
        ViewData["Amount"] = mostRecentOrder.TotalAmount;
        ViewData["orderId"] = orderId;
        IEnumerable<Category> categories = await _categoryRepository.GetAllCategoriesActiveAsync();
        ViewData["Categories"] = categories;
        return View("~/Views/UserOrder/Detail.cshtml");
        
    }

}
