using AppCore.App.Repositories;
using AppCore.Data;
using AppCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AppCore.Controllers;
public class ProfileController : Controller
{
    private readonly UserManager<Account> _accountManager;
    private CategoryRepository _categoryRepository;
    public ProfileController(UserManager<Account> accountManager, CategoryRepository categoryRepository)
    {
        _accountManager = accountManager;
        _categoryRepository = categoryRepository;
    }
    [Route("Profile/Index")]
    public async Task<IActionResult> Index()
    {
        var user = await _accountManager.GetUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }
        IEnumerable<Category> categories = await _categoryRepository.GetAllCategoriesActiveAsync();
        ViewData["Categories"] = categories;
        return View(user);
    }
    [HttpGet]
    [Route("Profile/Edit")]
    public IActionResult Edit()
    {
        return View();
    }
    [HttpPost]
    [Route("Profile/Update")]
    public async Task<IActionResult> Update(Account model, IFormFile ImageUpload)
    {
        if (ImageUpload != null && ImageUpload.Length > 0)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client/assets/img", ImageUpload.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                ImageUpload.CopyTo(stream);
            }
            model.Image = "/client/assets/img/" + ImageUpload.FileName;
        }
        else

        if (ModelState.IsValid)
        {
            var user = await _accountManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Image = model.Image;
            user.PhoneNumber = model.PhoneNumber;
            user.Address = model.Address;
            // Cập nhật các trường thông tin khác nếu cần

            var result = await _accountManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }
        // Output ModelState errors to the console
        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
        {
            Console.WriteLine(error.ErrorMessage);
        }

        return View("Edit", model);
    }

}
