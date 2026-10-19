using FUNewsManagement_BusinessObjects.Entities;
using FUNewsManagement_DataAccess.Repositories;

namespace FUNewsManagement_Services.Services;

public interface ITagService
{
    List<Tag> GetAllTags(string? searchTerm = null);
    Tag? GetTagById(int tagId);
    void AddTag(Tag tag);
    void UpdateTag(Tag tag);
    void DeleteTag(int tagId);
    int GetNextTagId();
}

public class TagService : ITagService
{
    private readonly ITagRepository _repository;

    public TagService(ITagRepository repository)
    {
        _repository = repository;
    }

    public List<Tag> GetAllTags(string? searchTerm = null) => _repository.GetAllTags(searchTerm);
    public Tag? GetTagById(int tagId) => _repository.GetTagById(tagId);
    public void AddTag(Tag tag) => _repository.AddTag(tag);
    public void UpdateTag(Tag tag) => _repository.UpdateTag(tag);
    public void DeleteTag(int tagId) => _repository.DeleteTag(tagId);
    public int GetNextTagId() => _repository.GetNextTagId();
}
