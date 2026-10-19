using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement_BusinessObjects.Entities;

public partial class Category
{
    public short CategoryId { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên danh mục.")]
    [StringLength(100, ErrorMessage = "Tên danh mục không được vượt quá 100 ký tự.")]
    public string CategoryName { get; set; } = null!;

    [Required(ErrorMessage = "Vui lòng nhập mô tả danh mục.")]
    [StringLength(250, ErrorMessage = "Mô tả danh mục không được vượt quá 250 ký tự.")]
    public string CategoryDesciption { get; set; } = null!;

    public short? ParentCategoryId { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Category> InverseParentCategory { get; set; } = new List<Category>();

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();

    public virtual Category? ParentCategory { get; set; }
}
