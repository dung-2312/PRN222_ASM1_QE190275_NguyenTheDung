using FUNewsManagement_BusinessObjects.Entities;
using FUNewsManagement_DataAccess.Repositories;

namespace FUNewsManagement_Services.Services;

public interface ICategoryService
{
    List<Category> GetCategories(string? searchTerm = null, bool onlyActive = false);
    Category? GetCategoryById(short categoryId);
    void AddCategory(Category category);
    void UpdateCategory(Category category);
    void DeleteCategory(short categoryId);
}

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public List<Category> GetCategories(string? searchTerm = null, bool onlyActive = false)
        => _repository.GetCategories(searchTerm, onlyActive);

    public Category? GetCategoryById(short categoryId)
        => _repository.GetCategoryById(categoryId);

    public void AddCategory(Category category)
        => _repository.AddCategory(category);

    public void UpdateCategory(Category category)
        => _repository.UpdateCategory(category);

    public void DeleteCategory(short categoryId)
        => _repository.DeleteCategory(categoryId);
}
