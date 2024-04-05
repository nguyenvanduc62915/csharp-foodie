using Microsoft.AspNetCore.Mvc;

using AppCore.Filters;
using AppCore.Models;
using AppCore.Models.ViewModels;
using AppCore.App.Repositories;

namespace AppCore.Controllers;

[AuthorizeAdmin]
// [Route("Admin/[controller]/[action]")]
public class CategoryController : Controller {
    private readonly ILogger<CategoryController> _logger;
    private CategoryRepository _categoryRepository;

    public CategoryController(ILogger<CategoryController> logger, CategoryRepository categoryRepository) {
        _categoryRepository = categoryRepository;
        _logger = logger;
    }

    [Route("Admin/Category")]
    public async Task<IActionResult> Index() {
        IEnumerable<Category> categories  = await _categoryRepository.GetAllCategoriesAsync();
        return View("~/Views/Admin/Category/Index.cshtml" , categories);
    }

    [Route("Admin/Category/Create")]
    public IActionResult Create() {
        string message = TempData["Message"] as string;
        // Gửi thông báo đến view
        ViewBag.Message = message;
        return View("~/Views/Admin/Category/Create.cshtml");
    }

    public async Task<IActionResult> Store(CategoryRequest categoryRequest, IFormFile ImageUpload)
    {
        try
        {
            if (ImageUpload != null && ImageUpload.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client/assets/img", ImageUpload.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    ImageUpload.CopyTo(stream);
                }
                categoryRequest.Image = "/client/assets/img/" + ImageUpload.FileName;
            }

            await _categoryRepository.AddCategoryAsync(new Category
            {
                Name = categoryRequest.Name,
                Image = categoryRequest.Image,
                Description = categoryRequest.Description,
                Active = categoryRequest.Active,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            });

            TempData["Message"] = "Tạo thành công";
            return RedirectToAction("Create");
        }
        catch (Exception e)
        {
            TempData["Message"] = "Tạo không thành công: " + e.Message;
            return RedirectToAction("Create");
        }
    }

    [Route("Admin/Category/Edit/{categoryId}")]
    public async Task<IActionResult> Edit(int categoryId) {
        var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
        if(category != null) {
            Console.WriteLine("có");
            ViewData["Category"] = category;
        }
        return View("~/Views/Admin/Category/Edit.cshtml");
    }
    [Route("Admin/Category/Update/{categoryId}")]
    public async Task<IActionResult> Update(int categoryId, CategoryRequest categoryRequest, IFormFile ImageUpload) {
        if (ImageUpload != null && ImageUpload.Length > 0)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "client/assets/img", ImageUpload.FileName);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                ImageUpload.CopyTo(stream);
            }
            categoryRequest.Image = "/client/assets/img/" + ImageUpload.FileName;
        }
		else
		{
			// If no new image is uploaded, keep the existing image path
			var existingProduct = await _categoryRepository.GetCategoryByIdAsync(categoryId);
			categoryRequest.Image = existingProduct.Image;
		}
		await _categoryRepository.UpdateCategoryAsync(new Category {
            CategoryId = categoryId,
            Name = categoryRequest.Name,
            Image = categoryRequest.Image,
            Description = categoryRequest.Description,
            Active = categoryRequest.Active,
            UpdatedAt = DateTime.Now
        });
        // AddCategoryAsync();
        TempData["Message"] = "Sửa thành công";
        return RedirectToAction("Index");
    }

    [Route("Admin/Category/Delete/{categoryId}")]
    public async Task<IActionResult> Delete(int categoryId) {
        var result = await _categoryRepository.DeleteCategoryAndAllProductAsync(categoryId);
        if(result) {
            return Json(new { success = true, message = "Xóa thành công." });
        }
        return Json(new { success = false, message = "Xóa ko thành công." });
    }
}
