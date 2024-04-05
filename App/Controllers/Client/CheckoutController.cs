using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

using AppCore.Models;
using AppCore.Models.ViewModels;
using AppCore.Filters;
using AppCore.App.Repositories;
using AppCore.App.Wrapper;
using AppCore.Extensions;
using AppCore.Data;

namespace AppCore.Controllers;

[AuthorizeUser]
public class CheckoutController : Controller {
    private readonly ILogger<CheckoutController> _logger;
    private OrderRepository _orderRepository;
    private OrderItemRepository _orderItemRepository;
    private CategoryRepository _categoryRepository;
    private PaymentMethodRepository _paymentMethodRepository;
    private readonly ApplicationDbContext _dbContext;

    public CheckoutController(ILogger<CheckoutController> logger, CategoryRepository categoryRepository, OrderRepository orderRepository, PaymentMethodRepository paymentMethodRepository, ApplicationDbContext dbContext, OrderItemRepository orderItemRepository) {
        _logger = logger;
        _orderRepository = orderRepository;
        _dbContext = dbContext;
        _orderItemRepository = orderItemRepository;
        _paymentMethodRepository = paymentMethodRepository;
        _categoryRepository = categoryRepository;
    }

    [Route("Cart/Checkout")]
    public async Task<IActionResult> Index() {
        var cart = HttpContext.Session.GetObject<Cart>("Cart");
        if(cart != null) {
            if(cart.OrderItems.Count > 0) {
                ViewData["PaymentMethods"]  = await _paymentMethodRepository.GetAllPaymentMethodsActiveAsync();
                var query = from orderItem in cart.OrderItems
                        join product in _dbContext.Products on orderItem.ProductId equals product.ProductId
                        select new OrderItemViewModel {
                            ProductId = orderItem.ProductId,
                            ProductName = product.Name,
                            Image = product.Image,
                            Price = product.Price,
                            Quantity = orderItem.Quantity,
                        };
                var orderItems = await Task.Run(() => query.ToList());
                ViewData["OrderItems"] = orderItems;
                IEnumerable<Category> categories = await _categoryRepository.GetAllCategoriesActiveAsync();
                ViewData["Categories"] = categories;
                return View();
            }
        }
        return RedirectToAction("Index", "Home");
    }

    // Cash Payment (default checkout)
    [Route("Cart/Checkout/PostCashPayment")]
    public async Task<IActionResult> Checkout(OrderRequest orderRequest) {
        string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var cart = HttpContext.Session.GetObject<Cart>("Cart");
        var newOrder = new Order {
            UserId = userId,
            PaymentMethodId = orderRequest.PaymentMethodId,
            ReceiverName = orderRequest.ReceiverName,
            ReceiverPhoneNumber = orderRequest.ReceiverPhoneNumber,
            ShippingAddress = orderRequest.ShippingAddress,
            TotalAmount = orderRequest.TotalAmount,
            Status = "Pending",
            PaymentStatus = PaymentStatus.Pending,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        await _orderRepository.AddOrderAsync(newOrder);
        var orderId = _dbContext.Entry(newOrder).Property("OrderId").CurrentValue;
       
        foreach (var orderItem in cart.OrderItems) {
            await _orderItemRepository.AddOrderItemAsync(new OrderItem{
                OrderId = (int)orderId,
                Quantity = orderItem.Quantity,
                Price = orderItem.Price,
                ProductId = orderItem.ProductId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });
        }
        cart.Clear();
        HttpContext.Session.Remove("Cart");
        HttpContext.Session.SetObject<Cart>("Cart", new Cart());


        TempData["SuccessMessage"] = "Thanh toán thành công";

        return RedirectToAction("Index", "Home");
        
    }

    public IActionResult MomoCheckout() {
        // implement momo
        return Json(new {});
    }

    public IActionResult VnPayCheckout() {
        // implement vnpay
        return Json(new {});

    }

    public IActionResult ReturnMomo() {
        // implement momo
        return Json(new {});
    }

     public IActionResult ReturnVnpay() {
        // implement momo
        return Json(new {});
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
