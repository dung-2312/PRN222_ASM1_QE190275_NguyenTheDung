using FUNewsManagement_BusinessObjects.Entities;

namespace FUNewsManagement_DataAccess.Repositories;

public interface ICategoryRepository
{
    List<Category> GetCategories(string? searchTerm = null, bool onlyActive = false);
    Category? GetCategoryById(short categoryId);
    void AddCategory(Category category);
    void UpdateCategory(Category category);
    void DeleteCategory(short categoryId);
}
