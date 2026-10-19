using System.Text;
using FUNewsManagement_BusinessObjects.Entities;
using FUNewsManagement_Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NguyenTheDungMVC.Models.Configuration;
using NguyenTheDungMVC.Models.ViewModels;

namespace NguyenTheDungMVC.Controllers;

public class NewsArticleController : Controller
{
    private readonly INewsArticleService _newsService;
    private readonly ICategoryService _categoryService;
    private readonly ITagService _tagService;
    private readonly ISystemAccountService _accountService;
    private readonly RoleSettingsOptions _roleOptions;

    public NewsArticleController(
        INewsArticleService newsService,
        ICategoryService categoryService,
        ITagService tagService,
        ISystemAccountService accountService,
        IOptions<RoleSettingsOptions> roleOptions)
    {
        _newsService = newsService;
        _categoryService = categoryService;
        _tagService = tagService;
        _accountService = accountService;
        _roleOptions = roleOptions.Value;
    }

    private bool IsAdmin() => HttpContext.Session.GetInt32(SessionKeys.AccountRole) == _roleOptions.AdminRole;
    private bool IsStaff() => HttpContext.Session.GetInt32(SessionKeys.AccountRole) == _roleOptions.StaffRole;

    [HttpGet]
    public IActionResult Index(string? searchTerm, short? categoryId)
    {
        ViewBag.SearchTerm = searchTerm;
        ViewBag.SelectedCategoryId = categoryId;
        ViewBag.Categories = _categoryService.GetCategories(onlyActive: true);

        var activeNews = _newsService.GetNewsArticles(searchTerm, onlyActive: true);
        if (categoryId.HasValue)
        {
            activeNews = activeNews.Where(n => n.CategoryId == categoryId.Value).ToList();
        }
        return View(activeNews);
    }

    [HttpGet]
    public IActionResult Details(string id)
    {
        if (string.IsNullOrWhiteSpace(id)) return NotFound();

        var article = _newsService.GetNewsArticleById(id);
        if (article == null) return NotFound();

        int? role = HttpContext.Session.GetInt32(SessionKeys.AccountRole);
        if (article.NewsStatus != true && role != _roleOptions.AdminRole && role != _roleOptions.StaffRole)
        {
            return RedirectToAction("AccessDenied", "Auth");
        }

        if (article.UpdatedById.HasValue)
        {
            var updater = _accountService.GetAccountById(article.UpdatedById.Value);
            ViewBag.UpdatedByName = updater?.AccountName;
        }

        return View(article);
    }

    [HttpGet]
    public IActionResult Manage(string? searchTerm, short? categoryId, bool? status)
    {
        if (!IsStaff()) return RedirectToAction("AccessDenied", "Auth");

        ViewBag.SearchTerm = searchTerm;
        ViewBag.SelectedCategoryId = categoryId;
        ViewBag.SelectedStatus = status;
        ViewBag.Categories = _categoryService.GetCategories(onlyActive: true);
        ViewBag.AllCategories = _categoryService.GetCategories();
        ViewBag.Tags = _tagService.GetAllTags();
        ViewBag.NextArticleId = _newsService.GenerateNextNewsArticleId();

        var articles = _newsService.GetNewsArticles(searchTerm, onlyActive: false);
        if (categoryId.HasValue)
        {
            articles = articles.Where(n => n.CategoryId == categoryId.Value).ToList();
        }
        if (status.HasValue)
        {
            articles = articles.Where(n => n.NewsStatus == status.Value).ToList();
        }

        return View(articles);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(NewsArticle article, List<int> selectedTagIds)
    {
        if (!IsStaff()) return RedirectToAction("AccessDenied", "Auth");

        short staffId = (short)(HttpContext.Session.GetInt32(SessionKeys.AccountId) ?? 0);
        article.CreatedDate = DateTime.Now;
        article.ModifiedDate = DateTime.Now;
        article.CreatedById = staffId;
        article.UpdatedById = staffId;

        if (!ModelState.IsValid)
        {
            var errors = string.Join(" ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            TempData["ErrorMessage"] = $"Dữ liệu không hợp lệ: {errors}";
            return RedirectToAction(nameof(Manage));
        }

        try
        {
            _newsService.AddNewsArticle(article, selectedTagIds);
            TempData["SuccessMessage"] = $"Đã đăng bài viết '{article.NewsTitle}' thành công.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Manage));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(NewsArticle article, List<int> selectedTagIds)
    {
        if (!IsStaff()) return RedirectToAction("AccessDenied", "Auth");

        short staffId = (short)(HttpContext.Session.GetInt32(SessionKeys.AccountId) ?? 0);
        article.ModifiedDate = DateTime.Now;
        article.UpdatedById = staffId;

        if (!ModelState.IsValid)
        {
            var errors = string.Join(" ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            TempData["ErrorMessage"] = $"Dữ liệu không hợp lệ: {errors}";
            return RedirectToAction(nameof(Manage));
        }

        try
        {
            _newsService.UpdateNewsArticle(article, selectedTagIds);
            TempData["SuccessMessage"] = $"Đã cập nhật bài viết '{article.NewsTitle}' thành công.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Manage));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(string newsArticleId)
    {
        if (!IsStaff()) return RedirectToAction("AccessDenied", "Auth");

        try
        {
            _newsService.DeleteNewsArticle(newsArticleId);
            TempData["SuccessMessage"] = "Đã xóa bài viết thành công.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Manage));
    }

    [HttpGet]
    public IActionResult History(string? searchTerm)
    {
        if (!IsStaff()) return RedirectToAction("AccessDenied", "Auth");

        short staffId = (short)(HttpContext.Session.GetInt32(SessionKeys.AccountId) ?? 0);
        ViewBag.SearchTerm = searchTerm;
        var myArticles = _newsService.GetNewsByCreator(staffId, searchTerm);
        return View(myArticles);
    }

    [HttpGet]
    public IActionResult Report(DateTime? startDate, DateTime? endDate)
    {
        if (!IsAdmin()) return RedirectToAction("AccessDenied", "Auth");

        if (startDate.HasValue && endDate.HasValue && startDate > endDate)
        {
            TempData["ErrorMessage"] = "Ngày bắt đầu không được lớn hơn ngày kết thúc.";
        }

        var articles = _newsService.GetNewsReportByPeriod(startDate, endDate);
        var model = new ReportViewModel
        {
            StartDate = startDate,
            EndDate = endDate,
            Articles = articles
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult ExportReportCsv(DateTime? startDate, DateTime? endDate)
    {
        if (!IsAdmin()) return RedirectToAction("AccessDenied", "Auth");

        var articles = _newsService.GetNewsReportByPeriod(startDate, endDate);
        var csv = new StringBuilder();
        csv.AppendLine("Mã bài viết,Ngày đăng,Tiêu đề,Tóm tắt,Danh mục,Tác giả,Trạng thái,Nhãn (Tags)");

        foreach (var item in articles)
        {
            string tags = string.Join(" | ", item.Tags.Select(t => t.TagName));
            string status = item.NewsStatus == true ? "Đang hiển thị" : "Tạm ẩn";
            csv.AppendLine($"\"{item.NewsArticleId}\",\"{item.CreatedDate:dd/MM/yyyy HH:mm}\",\"{EscapeCsv(item.NewsTitle)}\",\"{EscapeCsv(item.Headline)}\",\"{EscapeCsv(item.Category?.CategoryName)}\",\"{EscapeCsv(item.CreatedBy?.AccountName)}\",\"{status}\",\"{EscapeCsv(tags)}\"");
        }

        var bytes = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(csv.ToString())).ToArray();
        string fileName = $"BaoCaoTinTuc_FU_{DateTime.Now:yyyyMMdd_HHmm}.csv";
        return File(bytes, "text/csv", fileName);
    }

    private static string EscapeCsv(string? value)
        => string.IsNullOrEmpty(value) ? string.Empty : value.Replace("\"", "\"\"");
}
