using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement_BusinessObjects.Entities;

public partial class SystemAccount
{
    [Required(ErrorMessage = "Vui lòng nhập mã tài khoản.")]
    public short AccountId { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
    [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự.")]
    public string? AccountName { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập địa chỉ Email.")]
    [EmailAddress(ErrorMessage = "Định dạng địa chỉ Email không hợp lệ.")]
    [StringLength(70, ErrorMessage = "Địa chỉ Email không được vượt quá 70 ký tự.")]
    public string? AccountEmail { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn vai trò tài khoản.")]
    [Range(1, 2, ErrorMessage = "Vai trò phải là 1 (Nhân viên) hoặc 2 (Giảng viên).")]
    public int? AccountRole { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
    [StringLength(70, ErrorMessage = "Mật khẩu không được vượt quá 70 ký tự.")]
    public string? AccountPassword { get; set; }

    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
