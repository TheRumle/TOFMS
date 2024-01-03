namespace Tmpms.Common.JsonTofms.Models;

public record MoveActionDefinition(string Name, List<PartConsumptionDefinition> Parts, string From, string To,
    List<string> EmptyBefore, List<string> EmptyAfter)
{
    public override string ToString()
    {
        string partsString = string.Join(", ", Parts.Select(part => $"{part.Amount}'{part.PartType}"));
        string befString = string.Join(", ", EmptyBefore.Select(item => $"\"{item}\""));
        string afString = string.Join(", ", EmptyAfter.Select(item => $"\"{item}\""));

        return $"{nameof(Name)}: {Name}, {nameof(Parts)}: [{partsString}], {nameof(From)}: {From}, {nameof(To)}: {To}, {nameof(EmptyBefore)}: [{befString}], {nameof(EmptyAfter)}: [{afString}]";
    }
}