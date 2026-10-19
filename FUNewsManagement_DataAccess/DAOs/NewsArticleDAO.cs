using FUNewsManagement_BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagement_DataAccess.DAOs;

public class NewsArticleDAO
{
    private static NewsArticleDAO? instance = null;
    private static readonly object instanceLock = new object();

    private NewsArticleDAO() { }

    public static NewsArticleDAO Instance
    {
        get
        {
            lock (instanceLock)
            {
                instance ??= new NewsArticleDAO();
                return instance;
            }
        }
    }

    public List<NewsArticle> GetNewsArticles(string? searchTerm = null, bool onlyActive = false)
    {
        using var context = new FuNewsManagementContext();
        var query = context.NewsArticles
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.Tags)
            .AsQueryable();

        if (onlyActive)
        {
            query = query.Where(n => n.NewsStatus == true && (n.Category == null || n.Category.IsActive == true));
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string keyword = searchTerm.Trim().ToLower();
            query = query.Where(n =>
                (n.NewsTitle != null && n.NewsTitle.ToLower().Contains(keyword)) ||
                n.Headline.ToLower().Contains(keyword) ||
                (n.NewsContent != null && n.NewsContent.ToLower().Contains(keyword)) ||
                (n.Category != null && n.Category.CategoryName.ToLower().Contains(keyword)) ||
                n.Tags.Any(t => t.TagName != null && t.TagName.ToLower().Contains(keyword)));
        }

        return query.OrderByDescending(n => n.CreatedDate).ToList();
    }

    public NewsArticle? GetNewsArticleById(string newsArticleId)
    {
        using var context = new FuNewsManagementContext();
        return context.NewsArticles
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.Tags)
            .FirstOrDefault(n => n.NewsArticleId == newsArticleId);
    }

    public List<NewsArticle> GetNewsByCreator(short createdById, string? searchTerm = null)
    {
        using var context = new FuNewsManagementContext();
        var query = context.NewsArticles
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.Tags)
            .Where(n => n.CreatedById == createdById)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string keyword = searchTerm.Trim().ToLower();
            query = query.Where(n =>
                (n.NewsTitle != null && n.NewsTitle.ToLower().Contains(keyword)) ||
                n.Headline.ToLower().Contains(keyword));
        }

        return query.OrderByDescending(n => n.CreatedDate).ToList();
    }

    public List<NewsArticle> GetNewsReportByPeriod(DateTime? startDate, DateTime? endDate)
    {
        using var context = new FuNewsManagementContext();
        var query = context.NewsArticles
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.Tags)
            .AsQueryable();

        if (startDate.HasValue)
        {
            DateTime start = startDate.Value.Date;
            query = query.Where(n => n.CreatedDate >= start);
        }

        if (endDate.HasValue)
        {
            DateTime end = endDate.Value.Date.AddDays(1).AddTicks(-1);
            query = query.Where(n => n.CreatedDate <= end);
        }

        return query.OrderByDescending(n => n.CreatedDate).ToList();
    }

    public void AddNewsArticle(NewsArticle article, List<int> tagIds)
    {
        using var context = new FuNewsManagementContext();
        if (context.NewsArticles.Any(n => n.NewsArticleId == article.NewsArticleId))
        {
            throw new InvalidOperationException($"Mã bài viết '{article.NewsArticleId}' đã tồn tại.");
        }

        if (tagIds != null && tagIds.Any())
        {
            var tags = context.Tags.Where(t => tagIds.Contains(t.TagId)).ToList();
            article.Tags = tags;
        }

        context.NewsArticles.Add(article);
        context.SaveChanges();
    }

    public void UpdateNewsArticle(NewsArticle article, List<int> tagIds)
    {
        using var context = new FuNewsManagementContext();
        var existing = context.NewsArticles
            .Include(n => n.Tags)
            .FirstOrDefault(n => n.NewsArticleId == article.NewsArticleId)
            ?? throw new InvalidOperationException("Không tìm thấy bài viết.");

        existing.NewsTitle = article.NewsTitle;
        existing.Headline = article.Headline;
        existing.NewsContent = article.NewsContent;
        existing.NewsSource = article.NewsSource;
        existing.CategoryId = article.CategoryId;
        existing.NewsStatus = article.NewsStatus;
        existing.UpdatedById = article.UpdatedById;
        existing.ModifiedDate = article.ModifiedDate;

        existing.Tags.Clear();
        if (tagIds != null && tagIds.Any())
        {
            var selectedTags = context.Tags.Where(t => tagIds.Contains(t.TagId)).ToList();
            foreach (var tag in selectedTags)
            {
                existing.Tags.Add(tag);
            }
        }

        context.SaveChanges();
    }

    public void DeleteNewsArticle(string newsArticleId)
    {
        using var context = new FuNewsManagementContext();
        var existing = context.NewsArticles
            .Include(n => n.Tags)
            .FirstOrDefault(n => n.NewsArticleId == newsArticleId)
            ?? throw new InvalidOperationException("Không tìm thấy bài viết.");

        existing.Tags.Clear();
        context.NewsArticles.Remove(existing);
        context.SaveChanges();
    }

    public string GenerateNextNewsArticleId()
    {
        using var context = new FuNewsManagementContext();
        var ids = context.NewsArticles.Select(n => n.NewsArticleId).ToList();
        int maxNumeric = 0;
        foreach (var id in ids)
        {
            if (int.TryParse(id, out int num) && num > maxNumeric)
            {
                maxNumeric = num;
            }
        }
        return (maxNumeric + 1).ToString();
    }
}
