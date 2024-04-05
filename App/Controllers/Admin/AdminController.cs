using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using AppCore.Models.ViewModels;
using AppCore.Models;
using AppCore.Filters;
using AppCore.App.Repositories;
using Microsoft.EntityFrameworkCore;
using AppCore.Data;

namespace AppCore.Controllers;

public class AdminController : Controller {
    private readonly UserManager<Account> _adminManager;
    private readonly SignInManager<Account> _signInManager;
    private readonly ILogger<AdminController> _logger;
    private OrderRepository _orderRepository;
    private ProductRepository _productRepository;
    private readonly ApplicationDbContext _dbContext;

    public AdminController(ILogger<AdminController> logger,UserManager<Account> adminManager, 
        SignInManager<Account> signInManager, OrderRepository orderRepository, ProductRepository productRepository,ApplicationDbContext dbContext ) 
    {
        _logger = logger;
        _adminManager = adminManager;
        _signInManager = signInManager;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _dbContext = dbContext;
    }
    [AuthorizeAdmin]
    public async Task<IActionResult> Index() {
        var totalOrders = await _orderRepository.GetAllOrdersAsync();
        var totalOrdersCount = totalOrders.Count();
        var totalOrdersAmount = await GetTotalOrdersAmount();
        var totalProducts = GetTotalProducts(); // Gọi hàm tính tổng sản phẩm
        ViewBag.TotalProducts = totalProducts;

        ViewBag.TotalOrders = totalOrdersCount;
        ViewBag.TotalOrdersAmount = totalOrdersAmount;
        return View();
    }
    public int GetTotalProducts()
    {
        return _dbContext.Products.Count();
    }
    public async Task<decimal> GetTotalOrdersAmount()
    {
        var paidOrders = await _orderRepository.GetAllOrdersAsync();
        // Lọc chỉ những đơn hàng có trạng thái là "Paid"
        paidOrders = paidOrders.Where(order => order.Status == "Paid").ToList();
        decimal totalAmount = 0;
        foreach (var order in paidOrders)
        {
            totalAmount += order.TotalAmount;
        }

        return totalAmount;
    }
    public IActionResult Login() {
        var success = TempData["Success"] as string;
        ViewBag.Success = success;
        return View();
    }
    
    // [ValidateAntiForgeryToken]
    public async Task<IActionResult> PostLogin(LoginRequest loginRequest) {
        if (ModelState.IsValid) {
            Console.WriteLine(loginRequest.Email + " - "+ loginRequest.Password);
            var result = await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, isPersistent: loginRequest.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded) {
                var user = await _adminManager.FindByNameAsync(loginRequest.Email);
                var isAdmin = await _adminManager.IsInRoleAsync(user, "Admin");
                if(!isAdmin) {
                    return RedirectToAction("Logout");
                }
                return RedirectToAction("Index"); // Replace with the desired redirect URL
            }
            else if (result.IsLockedOut)
            {
                Console.WriteLine( "Account is locked out. Please try again later.");
                ModelState.AddModelError(string.Empty, "Account is locked out. Please try again later.");
            }
            else if (result.IsNotAllowed)
            {
                Console.WriteLine("Invalid login attempt. Please check your email for confirmation.");
                ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your email for confirmation.");
            }
            else { 
                Console.WriteLine( "Invalid login attempt.");
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
        }
        return RedirectToAction("Login");
    }

    public IActionResult Register() {
        var errors = TempData["Errors"] as string;
        // Truyền danh sách lỗi vào view để hiển thị
        ViewBag.Errors = errors;
        return View();
    }

    public async Task<IActionResult> PostRegister(RegisterRequest registerRequest) {
        var admin = new Admin {
            UserName = registerRequest.Email,
            FirstName = registerRequest.FirstName,
            LastName = registerRequest.LastName,
            PhoneNumber = registerRequest.PhoneNumber,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };

        var result = await _adminManager.CreateAsync(admin, registerRequest.Password);
        
        if (result.Succeeded) {
            await _adminManager.AddToRoleAsync(admin, "Admin");
            TempData["Success"] = "Register Successfully";
            return RedirectToAction("Login"); 
        }
        TempData["Errors"] = "Choose another email";
        return RedirectToAction("Register"); 
    }

    public async Task<IActionResult> Logout() {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login"); 
    }
}
