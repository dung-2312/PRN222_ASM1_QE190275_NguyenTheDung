using FUNewsManagement_BusinessObjects.Entities;
using FUNewsManagement_Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NguyenTheDungMVC.Models.Configuration;

namespace NguyenTheDungMVC.Controllers;

public class TagController : Controller
{
    private readonly ITagService _tagService;
    private readonly RoleSettingsOptions _roleOptions;

    public TagController(ITagService tagService, IOptions<RoleSettingsOptions> roleOptions)
    {
        _tagService = tagService;
        _roleOptions = roleOptions.Value;
    }

    private bool IsStaff() => HttpContext.Session.GetInt32(SessionKeys.AccountRole) == _roleOptions.StaffRole;

    [HttpGet]
    public IActionResult Index(string? searchTerm)
    {
        if (!IsStaff()) return RedirectToAction("AccessDenied", "Auth");

        ViewBag.SearchTerm = searchTerm;
        ViewBag.NextTagId = _tagService.GetNextTagId();
        var tags = _tagService.GetAllTags(searchTerm);
        return View(tags);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Tag tag)
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
            _tagService.AddTag(tag);
            TempData["SuccessMessage"] = $"Đã tạo nhãn '#{tag.TagName}' thành công.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Tag tag)
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
            _tagService.UpdateTag(tag);
            TempData["SuccessMessage"] = $"Đã cập nhật nhãn '#{tag.TagName}' thành công.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int tagId)
    {
        if (!IsStaff()) return RedirectToAction("AccessDenied", "Auth");

        try
        {
            _tagService.DeleteTag(tagId);
            TempData["SuccessMessage"] = "Đã xóa nhãn thành công.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}
