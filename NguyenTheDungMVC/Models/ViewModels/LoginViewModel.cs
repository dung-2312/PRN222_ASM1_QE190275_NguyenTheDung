using System.ComponentModel.DataAnnotations;

namespace NguyenTheDungMVC.Models.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập địa chỉ Email.")]
    [EmailAddress(ErrorMessage = "Vui lòng nhập đúng định dạng Email.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
