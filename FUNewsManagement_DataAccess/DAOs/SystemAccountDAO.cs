using FUNewsManagement_BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagement_DataAccess.DAOs;

public class SystemAccountDAO
{
    private static SystemAccountDAO? instance = null;
    private static readonly object instanceLock = new object();

    private SystemAccountDAO() { }

    public static SystemAccountDAO Instance
    {
        get
        {
            lock (instanceLock)
            {
                instance ??= new SystemAccountDAO();
                return instance;
            }
        }
    }

    public SystemAccount? GetAccountByEmailAndPassword(string email, string password)
    {
        using var context = new FuNewsManagementContext();
        return context.SystemAccounts
            .FirstOrDefault(a => a.AccountEmail != null &&
                                 a.AccountEmail.ToLower() == email.ToLower() &&
                                 a.AccountPassword == password);
    }

    public List<SystemAccount> GetAccounts(string? searchTerm = null)
    {
        using var context = new FuNewsManagementContext();
        var query = context.SystemAccounts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            string keyword = searchTerm.Trim().ToLower();
            query = query.Where(a =>
                (a.AccountName != null && a.AccountName.ToLower().Contains(keyword)) ||
                (a.AccountEmail != null && a.AccountEmail.ToLower().Contains(keyword)));
        }

        return query.OrderBy(a => a.AccountId).ToList();
    }

    public SystemAccount? GetAccountById(short accountId)
    {
        using var context = new FuNewsManagementContext();
        return context.SystemAccounts
            .Include(a => a.NewsArticles)
            .FirstOrDefault(a => a.AccountId == accountId);
    }

    public void AddAccount(SystemAccount account)
    {
        using var context = new FuNewsManagementContext();
        if (context.SystemAccounts.Any(a => a.AccountId == account.AccountId))
        {
            throw new InvalidOperationException($"Mã tài khoản #{account.AccountId} đã tồn tại trong hệ thống.");
        }
        if (context.SystemAccounts.Any(a => a.AccountEmail != null && account.AccountEmail != null && a.AccountEmail.ToLower() == account.AccountEmail.ToLower()))
        {
            throw new InvalidOperationException($"Địa chỉ Email '{account.AccountEmail}' đã được đăng ký.");
        }

        context.SystemAccounts.Add(account);
        context.SaveChanges();
    }

    public void UpdateAccount(SystemAccount account)
    {
        using var context = new FuNewsManagementContext();
        var existing = context.SystemAccounts.FirstOrDefault(a => a.AccountId == account.AccountId)
            ?? throw new InvalidOperationException("Không tìm thấy tài khoản.");

        if (context.SystemAccounts.Any(a => a.AccountId != account.AccountId &&
                                            a.AccountEmail != null &&
                                            account.AccountEmail != null &&
                                            a.AccountEmail.ToLower() == account.AccountEmail.ToLower()))
        {
            throw new InvalidOperationException($"Địa chỉ Email '{account.AccountEmail}' đã được sử dụng bởi tài khoản khác.");
        }

        existing.AccountName = account.AccountName;
        existing.AccountEmail = account.AccountEmail;
        existing.AccountRole = account.AccountRole;
        if (!string.IsNullOrWhiteSpace(account.AccountPassword))
        {
            existing.AccountPassword = account.AccountPassword;
        }

        context.SaveChanges();
    }

    public void DeleteAccount(short accountId)
    {
        using var context = new FuNewsManagementContext();
        var existing = context.SystemAccounts
            .Include(a => a.NewsArticles)
            .FirstOrDefault(a => a.AccountId == accountId)
            ?? throw new InvalidOperationException("Không tìm thấy tài khoản.");

        if (existing.NewsArticles.Any() || context.NewsArticles.Any(n => n.UpdatedById == accountId))
        {
            throw new InvalidOperationException("Không thể xóa tài khoản này vì tài khoản đã tham gia đăng hoặc cập nhật bài viết tin tức.");
        }

        context.SystemAccounts.Remove(existing);
        context.SaveChanges();
    }

    public short GetNextAccountId()
    {
        using var context = new FuNewsManagementContext();
        if (!context.SystemAccounts.Any()) return 1;
        return (short)(context.SystemAccounts.Max(a => a.AccountId) + 1);
    }
}
