using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement_BusinessObjects.Entities;

public partial class NewsArticle
{
    [Required(ErrorMessage = "Vui lòng nhập mã bài viết.")]
    [StringLength(20, ErrorMessage = "Mã bài viết không được vượt quá 20 ký tự.")]
    public string NewsArticleId { get; set; } = null!;

    [Required(ErrorMessage = "Vui lòng nhập tiêu đề bài viết.")]
    [StringLength(400, ErrorMessage = "Tiêu đề bài viết không được vượt quá 400 ký tự.")]
    public string? NewsTitle { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tóm tắt (Headline).")]
    [StringLength(150, ErrorMessage = "Tóm tắt không được vượt quá 150 ký tự.")]
    public string Headline { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập nội dung bài viết.")]
    [StringLength(4000, ErrorMessage = "Nội dung bài viết không được vượt quá 4000 ký tự.")]
    public string? NewsContent { get; set; }

    [StringLength(400, ErrorMessage = "Nguồn tin không được vượt quá 400 ký tự.")]
    public string? NewsSource { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn danh mục tin tức.")]
    public short? CategoryId { get; set; }

    public bool? NewsStatus { get; set; }

    public short? CreatedById { get; set; }

    public short? UpdatedById { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual Category? Category { get; set; }

    public virtual SystemAccount? CreatedBy { get; set; }

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
