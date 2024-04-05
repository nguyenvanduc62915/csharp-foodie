
using AppCore.Models;
namespace AppCore.App.Repositories;
public interface IProductRepository
{
    Task<Product> GetProductByIdAsync(int productId);
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task AddProductAsync(Product product);
    Task UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(int productId);
    Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId);
}