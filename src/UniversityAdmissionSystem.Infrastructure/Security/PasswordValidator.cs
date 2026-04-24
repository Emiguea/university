using UniversityAdmissionSystem.Core.Services;

namespace UniversityAdmissionSystem.Infrastructure.Security;

public static class PasswordValidator
{
    public static int MinimumLength = 8;
    public static int MaximumLength = 128;
    public static bool RequireUppercase = true;
    public static bool RequireLowercase = true;
    public static bool RequireDigit = true;
    public static bool RequireNonAlphanumeric = false;

    public static PasswordValidationResult Validate(string password)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(password))
        {
            errors.Add("密码不能为空");
            return new PasswordValidationResult { IsValid = false, Errors = errors };
        }

        if (password.Length < MinimumLength)
        {
            errors.Add($"密码长度至少需要 {MinimumLength} 个字符");
        }

        if (password.Length > MaximumLength)
        {
            errors.Add($"密码长度不能超过 {MaximumLength} 个字符");
        }

        if (RequireUppercase && !password.Any(char.IsUpper))
        {
            errors.Add("密码必须包含至少一个大写字母");
        }

        if (RequireLowercase && !password.Any(char.IsLower))
        {
            errors.Add("密码必须包含至少一个小写字母");
        }

        if (RequireDigit && !password.Any(char.IsDigit))
        {
            errors.Add("密码必须包含至少一个数字");
        }

        if (RequireNonAlphanumeric && !password.Any(c => !char.IsLetterOrDigit(c)))
        {
            errors.Add("密码必须包含至少一个特殊字符");
        }

        if (HasCommonPatterns(password))
        {
            errors.Add("密码包含常见的弱密码模式，请选择一个更强的密码");
        }

        return new PasswordValidationResult
        {
            IsValid = errors.Count == 0,
            Errors = errors
        };
    }

    private static bool HasCommonPatterns(string password)
    {
        var commonPatterns = new[]
        {
            "123456", "password", "admin", "root", "user",
            "qwerty", "asdfgh", "zxcvbn",
            "111111", "000000", "aaaaaa"
        };

        var lowerPassword = password.ToLower();
        return commonPatterns.Any(p => lowerPassword.Contains(p));
    }

    public static string GetPasswordRequirements()
    {
        var requirements = new List<string>
        {
            $"长度至少 {MinimumLength} 个字符"
        };

        if (RequireUppercase) requirements.Add("包含至少一个大写字母");
        if (RequireLowercase) requirements.Add("包含至少一个小写字母");
        if (RequireDigit) requirements.Add("包含至少一个数字");
        if (RequireNonAlphanumeric) requirements.Add("包含至少一个特殊字符");

        return $"密码要求：{string.Join("，", requirements)}";
    }
}
