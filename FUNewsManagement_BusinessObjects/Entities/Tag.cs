using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement_BusinessObjects.Entities;

public partial class Tag
{
    public int TagId { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên nhãn (Tag).")]
    [StringLength(50, ErrorMessage = "Tên nhãn không được vượt quá 50 ký tự.")]
    public string? TagName { get; set; }

    [StringLength(400, ErrorMessage = "Ghi chú không được vượt quá 400 ký tự.")]
    public string? Note { get; set; }

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
