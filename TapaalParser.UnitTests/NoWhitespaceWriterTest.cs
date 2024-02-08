using System.Text.RegularExpressions;

namespace TaapalParser.UnitTests;

public class NoWhitespaceWriterTest
{
    protected string RemoveWhiteSpace(string text)
    {
        return Regex.Replace(text, @"\s+", "");
    }
}