using FUNewsManagement_Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NguyenTheDungMVC.Models.Configuration;
using NguyenTheDungMVC.Models.ViewModels;

namespace NguyenTheDungMVC.Controllers;

public class AuthController : Controller
{
    private readonly ISystemAccountService _accountService;
    private readonly AdminAccountOptions _adminOptions;
    private readonly RoleSettingsOptions _roleOptions;

    public AuthController(
        ISystemAccountService accountService,
        IOptions<AdminAccountOptions> adminOptions,
        IOptions<RoleSettingsOptions> roleOptions)
    {
        _accountService = accountService;
        _adminOptions = adminOptions.Value;
        _roleOptions = roleOptions.Value;
    }

    [HttpGet]
    public IActionResult Login()
    {
        var role = HttpContext.Session.GetInt32(SessionKeys.AccountRole);
        if (role.HasValue)
        {
            return RedirectByRole(role.Value);
        }
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // 1. Kiểm tra tài khoản Quản trị viên từ appsettings.json qua IOptions
        if (!string.IsNullOrWhiteSpace(_adminOptions.Email) &&
            string.Equals(model.Email.Trim(), _adminOptions.Email, StringComparison.OrdinalIgnoreCase) &&
            model.Password == _adminOptions.Password)
        {
            HttpContext.Session.SetInt32(SessionKeys.AccountId, 0);
            HttpContext.Session.SetString(SessionKeys.AccountName, _adminOptions.AccountName);
            HttpContext.Session.SetString(SessionKeys.AccountEmail, _adminOptions.Email);
            HttpContext.Session.SetInt32(SessionKeys.AccountRole, _adminOptions.Role);

            return RedirectByRole(_adminOptions.Role);
        }

        // 2. Kiểm tra tài khoản Nhân viên / Giảng viên từ CSDL
        var account = _accountService.Authenticate(model.Email.Trim(), model.Password);
        if (account == null)
        {
            ViewBag.ErrorMessage = "Email hoặc mật khẩu không chính xác. Vui lòng kiểm tra lại.";
            return View(model);
        }

        int accountRole = account.AccountRole ?? _roleOptions.LecturerRole;
        HttpContext.Session.SetInt32(SessionKeys.AccountId, account.AccountId);
        HttpContext.Session.SetString(SessionKeys.AccountName, account.AccountName ?? account.AccountEmail ?? string.Empty);
        HttpContext.Session.SetString(SessionKeys.AccountEmail, account.AccountEmail ?? string.Empty);
        HttpContext.Session.SetInt32(SessionKeys.AccountRole, accountRole);

        return RedirectByRole(accountRole);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }

    public IActionResult AccessDenied()
    {
        return View();
    }

    private IActionResult RedirectByRole(int role)
    {
        if (role == _roleOptions.AdminRole)
            return RedirectToAction("Index", "SystemAccount");
        if (role == _roleOptions.StaffRole)
            return RedirectToAction("Manage", "NewsArticle");

        return RedirectToAction("Index", "NewsArticle");
    }
}
