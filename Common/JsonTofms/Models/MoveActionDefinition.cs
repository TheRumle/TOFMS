namespace Common.JsonTofms.Models;

public record PartConsumptionDefinition(int Amount, string PartType);

public record MoveActionDefinition(string Name, List<PartConsumptionDefinition> Parts, string From, string To, List<string> EmptyBefore, List<string> EmptyAfter);