using FUNewsManagement_BusinessObjects.Entities;
using FUNewsManagement_DataAccess.DAOs;

namespace FUNewsManagement_DataAccess.Repositories;

public interface ITagRepository
{
    List<Tag> GetAllTags(string? searchTerm = null);
    Tag? GetTagById(int tagId);
    void AddTag(Tag tag);
    void UpdateTag(Tag tag);
    void DeleteTag(int tagId);
    int GetNextTagId();
}

public class TagRepository : ITagRepository
{
    public List<Tag> GetAllTags(string? searchTerm = null) => TagDAO.Instance.GetAllTags(searchTerm);
    public Tag? GetTagById(int tagId) => TagDAO.Instance.GetTagById(tagId);
    public void AddTag(Tag tag) => TagDAO.Instance.AddTag(tag);
    public void UpdateTag(Tag tag) => TagDAO.Instance.UpdateTag(tag);
    public void DeleteTag(int tagId) => TagDAO.Instance.DeleteTag(tagId);
    public int GetNextTagId() => TagDAO.Instance.GetNextTagId();
}
