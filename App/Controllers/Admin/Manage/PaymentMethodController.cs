using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Authentication.Cookies;

using AppCore.Models;
using AppCore.Filters;
using AppCore.Models.ViewModels;
using AppCore.App.Repositories;
using AppCore.Data;

namespace AppCore.App.Controllers;

[AuthorizeAdmin]
public class PaymentMethodController : Controller {
    private readonly ILogger<PaymentMethodController> _logger;
    private PaymentMethodRepository _paymentMethodRepository;

    public PaymentMethodController(ILogger<PaymentMethodController> logger, PaymentMethodRepository paymentMethodRepository) {
        _logger = logger;
        _paymentMethodRepository = paymentMethodRepository;
    }

    [Route("Admin/Paymentmethod")]
    public async Task<IActionResult> Index() {
        string message = TempData["Message"] as string;
        ViewBag.Message = message;
        IEnumerable<PaymentMethod> paymentMethods  = await _paymentMethodRepository.GetAllPaymentMethodsAsync();
        return View("~/Views/Admin/PaymentMethod/Index.cshtml", paymentMethods);
    }
   
    [Route("Admin/Paymentmethod/Create")]
    public async Task<IActionResult> Create() {
        string message = TempData["Message"] as string;
        ViewBag.Message = message;
        return View("~/Views/Admin/Paymentmethod/Create.cshtml");
    }

    [Route("Admin/Paymentmethod/Store")]
    public async Task<IActionResult> Store(PaymentMethodRequest paymentMethodRequest) {
        await _paymentMethodRepository.AddPaymentMethodAsync(new PaymentMethod {
            Name = paymentMethodRequest.Name,
            Active = paymentMethodRequest.Active,
        });
        TempData["Message"] = "Tạo thành công";
        return RedirectToAction("Create");
    }

    [Route("Admin/PaymentMethod/Edit/{paymentMethodId}")]
    public async Task<IActionResult> Edit(int paymentMethodId) {
        var paymentMethod = await _paymentMethodRepository.GetPaymentMethodIdAsync(paymentMethodId);
        if(paymentMethod != null) {
            ViewData["PaymentMethod"] = paymentMethod;
            return View("~/Views/Admin/PaymentMethod/Edit.cshtml");
        }
        return View("~/Views/Admin/404.cshtml");
    }

    [Route("Admin/PaymentMethod/Update/{paymentMethodId}")]
    public async Task<IActionResult> Update(int paymentMethodId, PaymentMethodRequest paymentMethodRequest) {
        await _paymentMethodRepository.UpdatePaymentMethodAsync(new PaymentMethod {
            PaymentMethodId = paymentMethodId,
            Name = paymentMethodRequest.Name,
            Active = paymentMethodRequest.Active
        });
        TempData["Message"] = "Sửa thành công";
        return RedirectToAction("Index");
    }

    [Route("Admin/PaymentMethod/Delete/{paymentMethodId}")]
    public async Task<IActionResult> Delete(int paymentMethodId) {
        var result = await _paymentMethodRepository.DeletePaymentMethodAsync(paymentMethodId);
        if(result) {
            return Json(new { success = true, message = "Xóa thành công." });
        }
        return Json(new { success = false, message = "Xóa ko thành công." });
    }
}
