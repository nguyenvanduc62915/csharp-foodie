
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

using AppCore.Data;
using AppCore.Models;

namespace AppCore.App.Repositories;

public class ProductRepository : IProductRepository {
    private readonly ApplicationDbContext _dbContext;

    public ProductRepository(ApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<Product> GetProductByIdAsync(int productId)
    {
        return await _dbContext.Products.FindAsync(productId);
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _dbContext.Products.ToListAsync();
    }
    public async Task<IEnumerable<Product>> GetAllProductsActiveAsync() {
        return await _dbContext.Products
                                .Where(c => c.Active)  // Lọc theo trạng thái kích hoạt
                                .ToListAsync();
    }
	public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId)
	{
		return await _dbContext.Products
			.Where(p => p.CategoryId == categoryId)
			.OrderBy(p => p.Name)
			.ToListAsync();
	}
	public async Task AddProductAsync(Product product)
    {
        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateProductAsync(Product product)
    {
        var existringProduct = await GetProductByIdAsync(product.ProductId);

        if (existringProduct != null)
        {

            if (product.Image == null)
            {
                product.Image = existringProduct.Image;
            }
			_dbContext.Entry(existringProduct).CurrentValues.SetValues(product);
			await _dbContext.SaveChangesAsync();
		}
        
    }

    public async Task<bool> DeleteProductAsync(int productId) {
        var product = await _dbContext.Products.FindAsync(productId);
        if (product != null)
        {
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }

    Task<Product> IProductRepository.GetProductByIdAsync(int productId)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<Product>> IProductRepository.GetAllProductsAsync()
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