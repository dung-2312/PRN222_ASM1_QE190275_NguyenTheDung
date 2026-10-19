namespace NguyenTheDungMVC.Models.Configuration;

public class AdminAccountOptions
{
    public const string SectionName = "AdminAccount";
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public int Role { get; set; }
}

public class RoleSettingsOptions
{
    public const string SectionName = "RoleSettings";
    public int AdminRole { get; set; }
    public int StaffRole { get; set; }
    public int LecturerRole { get; set; }
}

public static class SessionKeys
{
    public const string AccountId = "AccountId";
    public const string AccountName = "AccountName";
    public const string AccountEmail = "AccountEmail";
    public const string AccountRole = "AccountRole";
}
