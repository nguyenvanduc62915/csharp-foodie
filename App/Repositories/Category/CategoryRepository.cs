
using Microsoft.EntityFrameworkCore;

using AppCore.Data;
using AppCore.Models;

namespace AppCore.App.Repositories;

public class CategoryRepository : ICategoryRepository {
    private readonly ApplicationDbContext _dbContext;

    public CategoryRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<Category> GetCategoryByIdAsync(int categoryId)
    {
        return await _dbContext.Categories.FindAsync(categoryId);
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _dbContext.Categories.ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesActiveAsync() {
        return await _dbContext.Categories.Where(c => c.Active).ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetSomeCategoriesAsync(int count)
    {
        return await _dbContext.Categories.Take(count).ToListAsync();
    }

    public async Task AddCategoryAsync(Category category)
    {
        _dbContext.Categories.Add(category);
        await _dbContext.SaveChangesAsync();
    }

	public async Task UpdateCategoryAsync(Category category)
	{
		var existingCategory = await GetCategoryByIdAsync(category.CategoryId);

		if (existingCategory != null)
		{
			// Preserve the existing image path if no new image is uploaded
			if (category.Image == null)
			{
				category.Image = existingCategory.Image;
			}
			_dbContext.Entry(existingCategory).CurrentValues.SetValues(category);
			await _dbContext.SaveChangesAsync();
		}
	}

	public async Task<bool> DeleteCategoryAsync(int categoryId) {
        var category = await _dbContext.Categories.FindAsync(categoryId);
        if (category != null)
        {
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> DeleteCategoryAndAllProductAsync(int categoryId) {
        var products =  _dbContext.Products.Where(p => p.CategoryId == categoryId).ToList();
        if(products.Count > 0) {
            foreach (var product in products) {
                _dbContext.Products.Remove(product);
            }
        }
        var category = await _dbContext.Categories.FindAsync(categoryId);
        if (category != null) {
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }

    Task<Category> ICategoryRepository.GetCategoryByIdAsync(int categoryId)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<Category>> ICategoryRepository.GetAllCategoriesAsync()
    {
        throw new NotImplementedException();
    }

    // public Task AddProductAsync(Product product)
    // {
    //     throw new NotImplementedException();
    // }

    // public Task UpdateProductAsync(Product product)
    // {
    //     throw new NotImplementedException();
    // }
}