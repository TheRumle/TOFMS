using Common.JsonTofms.Models;

namespace Common;

public record Invariant(string PartType, int Min, int Max)
{
    public Invariant(InvariantDefinition jsonInvariantDefinition): this(jsonInvariantDefinition.Part, jsonInvariantDefinition.Min, jsonInvariantDefinition.Max)
    {
    } 
};