using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using AppCore.Models;
using AppCore.Models.ViewModels;
using AppCore.Filters;
using AppCore.App.Repositories;
using AppCore.App.Wrapper;
using AppCore.Extensions;
using AppCore.Data;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace AppCore.Controllers;

public class CartController : Controller {
    private readonly ILogger<CartController> _logger;
    private PaymentMethodRepository _paymentMethodRepository;
    private readonly ApplicationDbContext _dbContext;
    private CategoryRepository _categoryRepository;
    private OrderRepository _orderRepository;

	public CartController(ILogger<CartController> logger, ApplicationDbContext dbContext , CategoryRepository categoryRepository, PaymentMethodRepository paymentMethodRepository, OrderRepository orderRepository) {
        _logger = logger;
        _orderRepository = orderRepository;
        _paymentMethodRepository = paymentMethodRepository;
        _dbContext = dbContext;
        _categoryRepository = categoryRepository;
    }
    
    public async Task<IActionResult> Index() {
        var cart = HttpContext.Session.GetObject<Cart>("Cart");
        if(cart == null) {
            HttpContext.Session.SetObject<Cart>("Cart", new Cart());
            cart = HttpContext.Session.GetObject<Cart>("Cart");
        }
        if(cart.OrderItems.Count > 0) {
            var query = from orderItem in cart.OrderItems
                        join product in _dbContext.Products on orderItem.ProductId equals product.ProductId
                        select new OrderItemViewModel {
                            ProductId = orderItem.ProductId,
                            ProductName = product.Name,
                            Image = product.Image,
                            Price = product.Price,
                            Quantity = orderItem.Quantity,
                        };
            // var orderItems = query.ToList();
            var orderItems = await Task.Run(() => query.ToList());
            ViewData["OrderItems"] = orderItems;
        }
        else {
            ViewData["OrderItems"] = new List<OrderItemViewModel>();
        }
        // ViewData["PaymentMethods"] = await _paymentMethodRepository.GetAllPaymentMethodsActiveAsync();
        IEnumerable<Category> categories = await _categoryRepository.GetAllCategoriesActiveAsync();
        ViewData["Categories"] = categories;
        return View();
    }

    [Route("Cart/Add/{productId}/{price}/{quantity}")]
    public IActionResult Add(int productId, decimal price) {
        
        var cart = HttpContext.Session.GetObject<Cart>("Cart");
        
        cart.AddItem(productId, price, 1);
        HttpContext.Session.SetObject<Cart>("Cart", cart);
        return Json(new { success = true, message = "thêm vào giỏ thành công." });
    }

    [Route("Cart/Remove/{productId}")]
    public IActionResult Delete(int productId) {
        var cart = HttpContext.Session.GetObject<Cart>("Cart");
        cart.RemoveItem(productId);
        HttpContext.Session.SetObject<Cart>("Cart", cart);

        return Json(new { success = true, message = "xóa item thành công." });
    }
    [Route("Cart/UpdateQuantity")]
    [HttpPost]
    public IActionResult UpdateQuantity(int productId, int newQuantity)
    {
        var cart = HttpContext.Session.GetObject<Cart>("Cart");

        // Tìm sản phẩm trong giỏ hàng và cập nhật số lượng
        var orderItem = cart.OrderItems.FirstOrDefault(item => item.ProductId == productId);
        if (orderItem != null)
        {
            orderItem.Quantity = newQuantity;
        }

        // Lưu giỏ hàng đã cập nhật vào Session
        HttpContext.Session.SetObject<Cart>("Cart", cart);

        // Trả về kết quả nếu cần thiết
        return Json(new { success = true, message = "Cập nhật số lượng thành công." });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
