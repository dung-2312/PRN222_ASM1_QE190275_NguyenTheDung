using FUNewsManagement_BusinessObjects.Entities;
using FUNewsManagement_DataAccess.DAOs;

namespace FUNewsManagement_DataAccess.Repositories;

public class CategoryRepository : ICategoryRepository
{
    public List<Category> GetCategories(string? searchTerm = null, bool onlyActive = false)
        => CategoryDAO.Instance.GetCategories(searchTerm, onlyActive);

    public Category? GetCategoryById(short categoryId)
        => CategoryDAO.Instance.GetCategoryById(categoryId);

    public void AddCategory(Category category)
        => CategoryDAO.Instance.AddCategory(category);

    public void UpdateCategory(Category category)
        => CategoryDAO.Instance.UpdateCategory(category);

    public void DeleteCategory(short categoryId)
        => CategoryDAO.Instance.DeleteCategory(categoryId);
}
