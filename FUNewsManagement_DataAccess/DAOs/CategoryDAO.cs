using FUNewsManagement_BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagement_DataAccess.DAOs;

public class CategoryDAO
{
    private static CategoryDAO? instance = null;
    private static readonly object instanceLock = new object();

    private CategoryDAO() { }

    public static CategoryDAO Instance
    {
        get
        {
            lock (instanceLock)
            {
                instance ??= new CategoryDAO();
                return instance;
            }
        }
    }

    public List<Category> GetCategories(string? searchTerm = null, bool onlyActive = false)
    {
        using var context = new FuNewsManagementContext();
        var query = context.Categories
            .Include(c => c.ParentCategory)
            .Include(c => c.NewsArticles)
            .AsQueryable();

        if (onlyActive)
        {
            query = query.Where(c => c.IsActive == true);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string keyword = searchTerm.Trim().ToLower();
            query = query.Where(c =>
                c.CategoryName.ToLower().Contains(keyword) ||
                c.CategoryDesciption.ToLower().Contains(keyword));
        }

        return query.OrderBy(c => c.CategoryId).ToList();
    }

    public Category? GetCategoryById(short categoryId)
    {
        using var context = new FuNewsManagementContext();
        return context.Categories
            .Include(c => c.ParentCategory)
            .Include(c => c.NewsArticles)
            .FirstOrDefault(c => c.CategoryId == categoryId);
    }

    public void AddCategory(Category category)
    {
        using var context = new FuNewsManagementContext();
        context.Categories.Add(category);
        context.SaveChanges();
    }

    public void UpdateCategory(Category category)
    {
        using var context = new FuNewsManagementContext();
        var existing = context.Categories.FirstOrDefault(c => c.CategoryId == category.CategoryId)
            ?? throw new InvalidOperationException("Không tìm thấy danh mục.");

        existing.CategoryName = category.CategoryName;
        existing.CategoryDesciption = category.CategoryDesciption;
        existing.ParentCategoryId = category.ParentCategoryId;
        existing.IsActive = category.IsActive;

        context.SaveChanges();
    }

    public void DeleteCategory(short categoryId)
    {
        using var context = new FuNewsManagementContext();
        var existing = context.Categories
            .Include(c => c.NewsArticles)
            .Include(c => c.InverseParentCategory)
            .FirstOrDefault(c => c.CategoryId == categoryId)
            ?? throw new InvalidOperationException("Không tìm thấy danh mục.");

        if (existing.NewsArticles.Any())
        {
            throw new InvalidOperationException("Không thể xóa danh mục này vì đã có bài viết tin tức thuộc danh mục.");
        }

        if (existing.InverseParentCategory.Any(sub => sub.CategoryId != categoryId))
        {
            throw new InvalidOperationException("Không thể xóa danh mục này vì đang là danh mục cha của danh mục khác.");
        }

        if (existing.ParentCategoryId == categoryId)
        {
            existing.ParentCategoryId = null;
            context.SaveChanges();
        }

        context.Categories.Remove(existing);
        context.SaveChanges();
    }
}
