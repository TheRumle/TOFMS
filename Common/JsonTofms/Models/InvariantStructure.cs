using Common.JsonTofms.ConsistencyCheck.Error;
using Newtonsoft.Json;

namespace Common.JsonTofms.Models;

public record InvariantStructure(string Part, int Min, int Max)
{
    public InvariantStructure((string name, int minValue, int maxValue) tuple): this(tuple.name,tuple.minValue,tuple.maxValue)
    {
        
    }
    
    [JsonConstructor]
    public InvariantStructure(string part, string min, string max): this(ParseMinMax(part, min, max))
    {
        
    }


    private static int ParseMax(string name, string min, string max)
    {
        var maxLower = max.ToLower();
        if (int.TryParse(max, out var maxValue))
            return maxValue;
        
        if (maxLower == "inf" || maxLower == "infty")
            return InfinityInteger.Positive;
        throw new InvalidInvariantException(name, min, max);
    }


    private static (string name, int minValue, int maxValue) ParseMinMax(string name, string min, string max)
    {
        int minValue = int.Parse(min);
        int maxValue = ParseMax(name, min, max);
        if (maxValue < minValue) throw new InvalidInvariantException(name, min, max);
        return (name, minValue, maxValue);
    }
}