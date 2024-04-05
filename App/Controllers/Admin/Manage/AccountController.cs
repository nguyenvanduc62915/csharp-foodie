using AppCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AppCore.App.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Account> _accountManager;

        public AccountController(UserManager<Account> accountManager)
        {
            _accountManager = accountManager;
        }

        [Route("Admin/Account")]
        public async Task<IActionResult> Index()
        {
            var user = await _accountManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        [HttpGet]
        [Route("/Admin/Account/Edit")]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        [Route("/Admin/Account/Update")]
        public async Task<IActionResult> Update(Account model, IFormFile ImageUpload)
        {
            if (ImageUpload != null && ImageUpload.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client/assets/img", ImageUpload.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await ImageUpload.CopyToAsync(stream);
                }
                model.Image = "/client/assets/img/" + ImageUpload.FileName;
            }

            if (ModelState.IsValid)
            {
                var user = await _accountManager.GetUserAsync(User);

                if (user == null)
                {
                    return NotFound();
                }
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Image = model.Image;
                user.Email = model.Email;
                user.Address = model.Address;
                user.PhoneNumber = model.PhoneNumber;
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
}