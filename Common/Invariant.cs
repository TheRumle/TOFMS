using Common.JsonTofms.Models;

namespace Common;

public record Invariant(string PartType, int Min, int Max)
{
    public Invariant(InvariantStructure jsonInvariantStructure): this(jsonInvariantStructure.PartType, jsonInvariantStructure.Min, jsonInvariantStructure.Max)
    {
    } 
};