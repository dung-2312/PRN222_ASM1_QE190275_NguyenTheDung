using FUNewsManagement_BusinessObjects.Entities;
using FUNewsManagement_Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NguyenTheDungMVC.Models.Configuration;

namespace NguyenTheDungMVC.Controllers;

public class SystemAccountController : Controller
{
    private readonly ISystemAccountService _accountService;
    private readonly RoleSettingsOptions _roleOptions;

    public SystemAccountController(
        ISystemAccountService accountService,
        IOptions<RoleSettingsOptions> roleOptions)
    {
        _accountService = accountService;
        _roleOptions = roleOptions.Value;
    }

    private bool IsAdmin() => HttpContext.Session.GetInt32(SessionKeys.AccountRole) == _roleOptions.AdminRole;
    private bool IsStaff() => HttpContext.Session.GetInt32(SessionKeys.AccountRole) == _roleOptions.StaffRole;

    [HttpGet]
    public IActionResult Index(string? searchTerm)
    {
        if (!IsAdmin()) return RedirectToAction("AccessDenied", "Auth");

        ViewBag.SearchTerm = searchTerm;
        ViewBag.NextAccountId = _accountService.GetNextAccountId();
        ViewBag.StaffRole = _roleOptions.StaffRole;
        ViewBag.LecturerRole = _roleOptions.LecturerRole;
        var accounts = _accountService.GetAccounts(searchTerm);
        return View(accounts);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(SystemAccount account)
    {
        if (!IsAdmin()) return RedirectToAction("AccessDenied", "Auth");

        if (!ModelState.IsValid)
        {
            var errors = string.Join(" ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            TempData["ErrorMessage"] = $"Dữ liệu không hợp lệ: {errors}";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            _accountService.AddAccount(account);
            TempData["SuccessMessage"] = $"Đã tạo tài khoản '{account.AccountName}' thành công.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(SystemAccount account)
    {
        if (!IsAdmin()) return RedirectToAction("AccessDenied", "Auth");

        if (!ModelState.IsValid)
        {
            var errors = string.Join(" ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            TempData["ErrorMessage"] = $"Dữ liệu không hợp lệ: {errors}";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            _accountService.UpdateAccount(account);
            TempData["SuccessMessage"] = $"Đã cập nhật tài khoản '{account.AccountName}' thành công.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(short accountId)
    {
        if (!IsAdmin()) return RedirectToAction("AccessDenied", "Auth");

        try
        {
            _accountService.DeleteAccount(accountId);
            TempData["SuccessMessage"] = "Đã xóa tài khoản thành công.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Profile()
    {
        if (!IsStaff()) return RedirectToAction("AccessDenied", "Auth");

        short accountId = (short)(HttpContext.Session.GetInt32(SessionKeys.AccountId) ?? 0);
        var account = _accountService.GetAccountById(accountId);
        if (account == null) return RedirectToAction("Login", "Auth");

        ViewBag.StaffRole = _roleOptions.StaffRole;
        return View(account);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateProfile(SystemAccount account)
    {
        if (!IsStaff()) return RedirectToAction("AccessDenied", "Auth");

        short sessionAccountId = (short)(HttpContext.Session.GetInt32(SessionKeys.AccountId) ?? 0);
        account.AccountId = sessionAccountId;
        account.AccountRole = _roleOptions.StaffRole;

        if (!ModelState.IsValid)
        {
            var errors = string.Join(" ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            TempData["ErrorMessage"] = $"Dữ liệu không hợp lệ: {errors}";
            return RedirectToAction(nameof(Profile));
        }

        try
        {
            _accountService.UpdateAccount(account);
            HttpContext.Session.SetString(SessionKeys.AccountName, account.AccountName ?? string.Empty);
            HttpContext.Session.SetString(SessionKeys.AccountEmail, account.AccountEmail ?? string.Empty);
            TempData["SuccessMessage"] = "Cập nhật hồ sơ cá nhân thành công.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Profile));
    }
}
