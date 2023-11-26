using System.Text.RegularExpressions;

namespace Tofms.Common;

internal static class StringExtensions
{
    private static readonly Regex alphaNumericOnly = new("^[a-zA-Z0-9]+$");

    internal static bool IsAlphaNumericOnly(this string s)
    {
        return alphaNumericOnly.IsMatch(s);
    }
}