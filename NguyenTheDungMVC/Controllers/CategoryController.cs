using FUNewsManagement_BusinessObjects.Entities;
using FUNewsManagement_Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NguyenTheDungMVC.Models.Configuration;

namespace NguyenTheDungMVC.Controllers;

public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly RoleSettingsOptions _roleOptions;

    public CategoryController(
        ICategoryService categoryService,
        IOptions<RoleSettingsOptions> roleOptions)
    {
        _categoryService = categoryService;
        _roleOptions = roleOptions.Value;
    }

    private bool IsStaff() => HttpContext.Session.GetInt32(SessionKeys.AccountRole) == _roleOptions.StaffRole;

    [HttpGet]
    public IActionResult Index(string? searchTerm)
    {
        if (!IsStaff()) return RedirectToAction("AccessDenied", "Auth");

        ViewBag.SearchTerm = searchTerm;
        ViewBag.AllCategories = _categoryService.GetCategories();
        var categories = _categoryService.GetCategories(searchTerm);
        return View(categories);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category category)
    {
        if (!IsStaff()) return RedirectToAction("AccessDenied", "Auth");

        if (!ModelState.IsValid)
        {
            var errors = string.Join(" ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            TempData["ErrorMessage"] = $"Dữ liệu không hợp lệ: {errors}";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            category.IsActive ??= true;
            _categoryService.AddCategory(category);
            TempData["SuccessMessage"] = $"Đã tạo danh mục '{category.CategoryName}' thành công.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Category category)
    {
        if (!IsStaff()) return RedirectToAction("AccessDenied", "Auth");

        if (!ModelState.IsValid)
        {
            var errors = string.Join(" ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            TempData["ErrorMessage"] = $"Dữ liệu không hợp lệ: {errors}";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            _categoryService.UpdateCategory(category);
            TempData["SuccessMessage"] = $"Đã cập nhật danh mục '{category.CategoryName}' thành công.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(short categoryId)
    {
        if (!IsStaff()) return RedirectToAction("AccessDenied", "Auth");

        try
        {
            _categoryService.DeleteCategory(categoryId);
            TempData["SuccessMessage"] = "Đã xóa danh mục thành công.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}
