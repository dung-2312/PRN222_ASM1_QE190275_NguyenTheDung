using FUNewsManagement_BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagement_DataAccess.DAOs;

public class TagDAO
{
    private static TagDAO? instance = null;
    private static readonly object instanceLock = new object();

    private TagDAO() { }

    public static TagDAO Instance
    {
        get
        {
            lock (instanceLock)
            {
                instance ??= new TagDAO();
                return instance;
            }
        }
    }

    public List<Tag> GetAllTags(string? searchTerm = null)
    {
        using var context = new FuNewsManagementContext();
        var query = context.Tags
            .Include(t => t.NewsArticles)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string keyword = searchTerm.Trim().ToLower();
            query = query.Where(t =>
                (t.TagName != null && t.TagName.ToLower().Contains(keyword)) ||
                (t.Note != null && t.Note.ToLower().Contains(keyword)));
        }

        return query.OrderBy(t => t.TagId).ToList();
    }

    public Tag? GetTagById(int tagId)
    {
        using var context = new FuNewsManagementContext();
        return context.Tags
            .Include(t => t.NewsArticles)
            .FirstOrDefault(t => t.TagId == tagId);
    }

    public void AddTag(Tag tag)
    {
        using var context = new FuNewsManagementContext();
        if (context.Tags.Any(t => t.TagId == tag.TagId))
        {
            throw new InvalidOperationException($"Mã nhãn #{tag.TagId} đã tồn tại.");
        }
        if (context.Tags.Any(t => t.TagName != null && tag.TagName != null && t.TagName.ToLower() == tag.TagName.Trim().ToLower()))
        {
            throw new InvalidOperationException($"Tên nhãn '{tag.TagName}' đã tồn tại.");
        }

        tag.TagName = tag.TagName?.Trim();
        context.Tags.Add(tag);
        context.SaveChanges();
    }

    public void UpdateTag(Tag tag)
    {
        using var context = new FuNewsManagementContext();
        var existing = context.Tags.FirstOrDefault(t => t.TagId == tag.TagId)
            ?? throw new InvalidOperationException("Không tìm thấy nhãn.");

        if (context.Tags.Any(t => t.TagId != tag.TagId && t.TagName != null && tag.TagName != null && t.TagName.ToLower() == tag.TagName.Trim().ToLower()))
        {
            throw new InvalidOperationException($"Tên nhãn '{tag.TagName}' đã tồn tại.");
        }

        existing.TagName = tag.TagName?.Trim();
        existing.Note = tag.Note;
        context.SaveChanges();
    }

    public void DeleteTag(int tagId)
    {
        using var context = new FuNewsManagementContext();
        var existing = context.Tags
            .Include(t => t.NewsArticles)
            .FirstOrDefault(t => t.TagId == tagId)
            ?? throw new InvalidOperationException("Không tìm thấy nhãn.");

        if (existing.NewsArticles.Any())
        {
            throw new InvalidOperationException("Không thể xóa nhãn này vì đang được gắn vào một hoặc nhiều bài viết.");
        }

        context.Tags.Remove(existing);
        context.SaveChanges();
    }

    public int GetNextTagId()
    {
        using var context = new FuNewsManagementContext();
        if (!context.Tags.Any()) return 1;
        return context.Tags.Max(t => t.TagId) + 1;
    }
}
