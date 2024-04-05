using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using AppCore.Models;
using AppCore.Filters;
using AppCore.App.Repositories;
using AppCore.App.Wrapper;
using AppCore.Extensions;
using Microsoft.EntityFrameworkCore;
using AppCore.Data;

using System.Drawing.Printing;
namespace AppCore.Controllers;

public class HomeController : Controller
{
	private readonly ILogger<HomeController> _logger;
	private ProductRepository _productRepository;
	private CategoryRepository _categoryRepository;
	private readonly ApplicationDbContext _dbContext;

	public HomeController(ILogger<HomeController> logger, ProductRepository productRepository, CategoryRepository categoryRepository, ApplicationDbContext dbContext)
	{
		_logger = logger;
		_productRepository = productRepository;
		_categoryRepository = categoryRepository;
		this._dbContext = dbContext;
		dbContext.Database.EnsureCreated();
	}

	public async Task<IActionResult> Index(string searchTerm) 
    {
        
		var cart = HttpContext.Session.GetObject<Cart>("Cart");
        IEnumerable<Category> categories = await _categoryRepository.GetAllCategoriesActiveAsync();

        if (!string.IsNullOrEmpty(searchTerm))
		{

			var searchResults = _dbContext.Products
				.Where(p => p.Name.ToUpper().Contains(searchTerm.ToUpper()))
				.ToList();

			ViewData["SearchResults"] = searchResults;
            ViewData["Categories"] = categories;
            return View("SearchResults", searchResults);
		}		
		else
		if (cart == null) {
            HttpContext.Session.SetObject<Cart>("Cart", new Cart());
            cart = HttpContext.Session.GetObject<Cart>("Cart");
        }
        IEnumerable<Product> products  = await _productRepository.GetAllProductsAsync();
        ViewData["Categories"] = categories;
        ViewData["Products"] = products;
        return View();
    }

    // [AuthorizeUser]
    public IActionResult Privacy() {       
        return View();
    }

    [AuthorizeUser]
    public IActionResult About() {
         return View("~/Views/Home/About.cshtml");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

	public async Task<IActionResult> SPTheoDanhMuc(int Id)
	{
		var cart = HttpContext.Session.GetObject<Cart>("Cart");
		IEnumerable<Product> products = await _productRepository.GetProductsByCategoryIdAsync(Id);
        IEnumerable<Category> categories = await _categoryRepository.GetAllCategoriesActiveAsync();
		ViewData["Categories"] = categories;
		ViewData["Products"] = products;
		return View(products);
	}


	public async Task<IActionResult> SingleProduct(int productId)
    {
        if (productId == null)
        {
            return NotFound("Product ID is required.");
        }

        var product = _dbContext.Products
                        .Include(p => p.Category)
                        .FirstOrDefault(m => m.ProductId == productId);

        if (product == null)
        {
            return NotFound("Product not found.");
        }
        IEnumerable<Category> categories = await _categoryRepository.GetAllCategoriesActiveAsync();
        ViewData["Categories"] = categories;
        return View("SingleProduct", product);
    }
	
	
}
