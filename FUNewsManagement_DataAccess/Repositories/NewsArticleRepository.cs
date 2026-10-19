using FUNewsManagement_BusinessObjects.Entities;
using FUNewsManagement_DataAccess.DAOs;

namespace FUNewsManagement_DataAccess.Repositories;

public class NewsArticleRepository : INewsArticleRepository
{
    public List<NewsArticle> GetNewsArticles(string? searchTerm = null, bool onlyActive = false)
        => NewsArticleDAO.Instance.GetNewsArticles(searchTerm, onlyActive);

    public NewsArticle? GetNewsArticleById(string newsArticleId)
        => NewsArticleDAO.Instance.GetNewsArticleById(newsArticleId);

    public List<NewsArticle> GetNewsByCreator(short createdById, string? searchTerm = null)
        => NewsArticleDAO.Instance.GetNewsByCreator(createdById, searchTerm);

    public List<NewsArticle> GetNewsReportByPeriod(DateTime? startDate, DateTime? endDate)
        => NewsArticleDAO.Instance.GetNewsReportByPeriod(startDate, endDate);

    public void AddNewsArticle(NewsArticle article, List<int> tagIds)
        => NewsArticleDAO.Instance.AddNewsArticle(article, tagIds);

    public void UpdateNewsArticle(NewsArticle article, List<int> tagIds)
        => NewsArticleDAO.Instance.UpdateNewsArticle(article, tagIds);

    public void DeleteNewsArticle(string newsArticleId)
        => NewsArticleDAO.Instance.DeleteNewsArticle(newsArticleId);

    public string GenerateNextNewsArticleId()
        => NewsArticleDAO.Instance.GenerateNextNewsArticleId();
}
