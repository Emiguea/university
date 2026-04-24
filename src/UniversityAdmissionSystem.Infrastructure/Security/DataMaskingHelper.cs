namespace UniversityAdmissionSystem.Infrastructure.Security;

public static class DataMaskingHelper
{
    public static string MaskIdCard(string? idCard)
    {
        if (string.IsNullOrWhiteSpace(idCard))
            return string.Empty;

        if (idCard.Length < 8)
            return new string('*', idCard.Length);

        var prefix = idCard.Substring(0, 6);
        var suffix = idCard.Substring(idCard.Length - 4);
        var middleLength = idCard.Length - 10;
        var middle = new string('*', middleLength);

        return $"{prefix}{middle}{suffix}";
    }

    public static string MaskPhoneNumber(string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            return string.Empty;

        if (phoneNumber.Length < 7)
            return new string('*', phoneNumber.Length);

        var prefix = phoneNumber.Substring(0, 3);
        var suffix = phoneNumber.Substring(phoneNumber.Length - 4);
        var middleLength = phoneNumber.Length - 7;
        var middle = new string('*', middleLength);

        return $"{prefix}{middle}{suffix}";
    }

    public static string MaskEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return string.Empty;

        var atIndex = email.IndexOf('@');
        if (atIndex <= 0)
            return email;

        var username = email.Substring(0, atIndex);
        var domain = email.Substring(atIndex);

        if (username.Length <= 2)
            return new string('*', username.Length) + domain;

        var prefix = username.Substring(0, 1);
        var suffix = username.Substring(username.Length - 1);
        var middle = new string('*', username.Length - 2);

        return $"{prefix}{middle}{suffix}{domain}";
    }

    public static string MaskAddress(string? address)
    {
        if (string.IsNullOrWhiteSpace(address))
            return string.Empty;

        if (address.Length < 6)
            return new string('*', address.Length);

        var visibleLength = Math.Min(address.Length / 2, 8);
        var prefix = address.Substring(0, visibleLength);
        var suffix = address.Substring(address.Length - visibleLength);
        var middle = new string('*', address.Length - visibleLength * 2);

        return $"{prefix}{middle}{suffix}";
    }
}
