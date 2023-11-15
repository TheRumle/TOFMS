using System.Text.RegularExpressions;

namespace Common;

internal static class StringExtensions
{
    private static Regex alphaNumericOnly = new Regex("^[a-zA-Z0-9 ]*$");

    internal static bool IsAlphaNumericOnly(this string s)
    {
        return alphaNumericOnly.IsMatch(s);
    }
}