using System.Text.RegularExpressions;

namespace Common.Move;

public static class StringExtensions
{
    private static Regex alphaNumericOnly = new Regex("^[a-zA-Z0-9 ]*$");

    public static bool IsAlphaNumericOnly(this string s)
    {
        return alphaNumericOnly.IsMatch(s);
    }
}