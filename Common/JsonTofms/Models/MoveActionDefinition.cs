namespace Common.JsonTofms.Models;

public record MoveActionDefinition(string Name, List<(int Amount, string PartType)> Parts, string From, string To, List<string> EmptyBefore, List<string> EmptyAfter);