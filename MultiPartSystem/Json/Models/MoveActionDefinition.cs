namespace Tmpms.Common.Json.Models;

public record MoveActionDefinition(string Name, Dictionary<string, int> Parts, string From, string To,
    List<string> EmptyBefore, List<string> EmptyAfter)
{
    public override string ToString()
    {
        string partsString = string.Join(", ", Parts.Select(part => $"{part.Key}'{part.Value}"));
        string befString = string.Join(", ", EmptyBefore.Select(item => $"\"{item}\""));
        string afString = string.Join(", ", EmptyAfter.Select(item => $"\"{item}\""));

        return $"{nameof(Name)}: {Name}, {nameof(Parts)}: [{partsString}], {nameof(From)}: {From}, {nameof(To)}: {To}, {nameof(EmptyBefore)}: [{befString}], {nameof(EmptyAfter)}: [{afString}]";
    }
}


