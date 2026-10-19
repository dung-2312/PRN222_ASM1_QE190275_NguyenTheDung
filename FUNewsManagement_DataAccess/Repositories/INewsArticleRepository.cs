using FUNewsManagement_BusinessObjects.Entities;

namespace FUNewsManagement_DataAccess.Repositories;

public interface INewsArticleRepository
{
    List<NewsArticle> GetNewsArticles(string? searchTerm = null, bool onlyActive = false);
    NewsArticle? GetNewsArticleById(string newsArticleId);
    List<NewsArticle> GetNewsByCreator(short createdById, string? searchTerm = null);
    List<NewsArticle> GetNewsReportByPeriod(DateTime? startDate, DateTime? endDate);
    void AddNewsArticle(NewsArticle article, List<int> tagIds);
    void UpdateNewsArticle(NewsArticle article, List<int> tagIds);
    void DeleteNewsArticle(string newsArticleId);
    string GenerateNextNewsArticleId();
}
