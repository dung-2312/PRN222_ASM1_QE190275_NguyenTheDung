using FUNewsManagement_BusinessObjects.Entities;
using FUNewsManagement_DataAccess.Repositories;

namespace FUNewsManagement_Services.Services;

public interface INewsArticleService
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

public class NewsArticleService : INewsArticleService
{
    private readonly INewsArticleRepository _repository;

    public NewsArticleService(INewsArticleRepository repository)
    {
        _repository = repository;
    }

    public List<NewsArticle> GetNewsArticles(string? searchTerm = null, bool onlyActive = false)
        => _repository.GetNewsArticles(searchTerm, onlyActive);

    public NewsArticle? GetNewsArticleById(string newsArticleId)
        => _repository.GetNewsArticleById(newsArticleId);

    public List<NewsArticle> GetNewsByCreator(short createdById, string? searchTerm = null)
        => _repository.GetNewsByCreator(createdById, searchTerm);

    public List<NewsArticle> GetNewsReportByPeriod(DateTime? startDate, DateTime? endDate)
        => _repository.GetNewsReportByPeriod(startDate, endDate);

    public void AddNewsArticle(NewsArticle article, List<int> tagIds)
        => _repository.AddNewsArticle(article, tagIds);

    public void UpdateNewsArticle(NewsArticle article, List<int> tagIds)
        => _repository.UpdateNewsArticle(article, tagIds);

    public void DeleteNewsArticle(string newsArticleId)
        => _repository.DeleteNewsArticle(newsArticleId);

    public string GenerateNextNewsArticleId()
        => _repository.GenerateNextNewsArticleId();
}
