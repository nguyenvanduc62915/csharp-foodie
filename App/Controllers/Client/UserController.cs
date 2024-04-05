using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using AppCore.Models.ViewModels;
using AppCore.Models;
using AppCore.Extensions;

namespace AppCore.Client.Controllers;

public class UserController : Controller {
    private readonly UserManager<Account> _userManager;
    private readonly SignInManager<Account> _signInManager;
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger, UserManager<Account> userManager, SignInManager<Account> signInManager) {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Login() {
        string message = TempData["Message"] as string;
        ViewBag.Message = message;
        return View();
    }

    public async Task<IActionResult> PostLogin(LoginRequest loginRequest) {
        if (ModelState.IsValid) {
            Console.WriteLine(loginRequest.Email + " - "+ loginRequest.Password);
            var result = await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, isPersistent: loginRequest.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded) {
                // var user = await _userManager.FindByNameAsync(loginRequest.Email);
                // HttpContext.Session.SetObject("CurrentUser", user);
                return RedirectToAction("Index", "Home"); 
            }
            else {
                Console.WriteLine( "Invalid login attempt.");
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
        }
        return RedirectToAction("Login");
    }
    
    public IActionResult Register() {
        return View();
    }

    public async Task<IActionResult> PostRegister(RegisterRequest registerRequest) {
        if (ModelState.IsValid) {
            if(registerRequest != null) {
                var user = new User {
                    UserName = registerRequest.Email,
                    Email = registerRequest.Email,
                    FirstName = registerRequest.FirstName,
                    LastName = registerRequest.LastName,
                    PhoneNumber = registerRequest.PhoneNumber,
                    Address = registerRequest.Address,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                var result = await _userManager.CreateAsync(user, registerRequest.Password);
                if (result.Succeeded) {
                    await _userManager.AddToRoleAsync(user, "User");
                    TempData["Message"] = "Tạo thành công";
                    return RedirectToAction("Login"); 
                }
                else {
                    Console.WriteLine( "Invalid login attempt.");
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }
        }
        return RedirectToAction("Register");
    }
    
    public async Task<IActionResult> Logout() {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login"); 
    }

}
