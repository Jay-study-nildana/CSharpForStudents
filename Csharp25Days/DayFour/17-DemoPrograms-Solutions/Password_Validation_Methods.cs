using System;
using System.Linq;
using System.Text.RegularExpressions;

class Password_Validation_Methods
{
    // Small reusable validators; compose them in PasswordStrength
    // Time: O(n) per check, Space: O(1) extra

    static bool HasLength(string p, int min) => p != null && p.Length >= min;

    static bool HasUpperLowerDigit(string p)
    {
        if (string.IsNullOrEmpty(p)) return false;
        bool hasUpper = false, hasLower = false, hasDigit = false;
        foreach (var c in p)
        {
            if (char.IsUpper(c)) hasUpper = true;
            else if (char.IsLower(c)) hasLower = true;
            else if (char.IsDigit(c)) hasDigit = true;
            if (hasUpper && hasLower && hasDigit) return true;
        }
        return false;
    }

    static bool HasSpecialChar(string p)
    {
        if (string.IsNullOrEmpty(p)) return false;
        return Regex.IsMatch(p, @"[!@#$%^&*(),.?:{}|<>]");
    }

    // Compose validators to compute a score 0..4
    static int PasswordStrength(string p)
    {
        int score = 0;
        if (HasLength(p, 8)) score++;
        if (HasUpperLowerDigit(p)) score++;
        if (HasSpecialChar(p)) score++;
        if (p != null && p.Length >= 12) score++; // extra for length
        return score;
    }

    static void Main()
    {
        var passwords = new[] { "pass", "Password1", "P@ssw0rd", "Very$trongPassword123" };
        foreach (var pw in passwords)
        {
            Console.WriteLine($"'{pw}' => Score: {PasswordStrength(pw)}");
        }
    }
}