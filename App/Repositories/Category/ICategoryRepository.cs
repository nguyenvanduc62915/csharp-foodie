
using AppCore.Models;

namespace AppCore.App.Repositories;

public interface ICategoryRepository {

    Task<Category> GetCategoryByIdAsync(int productId);
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task AddCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category);
    Task<bool> DeleteCategoryAsync(int categorytId);

}